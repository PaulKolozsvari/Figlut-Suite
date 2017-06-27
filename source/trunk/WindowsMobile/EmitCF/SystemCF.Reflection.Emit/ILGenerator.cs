using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;
using SystemCF.Runtime.InteropServices;
using SystemCF.Diagnostics.SymbolStore;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Generates Microsoft intermediate language (MSIL) instructions. 
	/// </summary>
	public class ILGenerator
	{
		#region Private&Internal
		static Mono.Cecil.Cil.OpCode[] Ldlocs = new Mono.Cecil.Cil.OpCode[] {
			Mono.Cecil.Cil.OpCodes.Ldloc_0,
			Mono.Cecil.Cil.OpCodes.Ldloc_1,
			Mono.Cecil.Cil.OpCodes.Ldloc_2,
			Mono.Cecil.Cil.OpCodes.Ldloc_3
		};

		static Mono.Cecil.Cil.OpCode[] Stlocs = new Mono.Cecil.Cil.OpCode[] {
			Mono.Cecil.Cil.OpCodes.Stloc_0,
			Mono.Cecil.Cil.OpCodes.Stloc_1,
			Mono.Cecil.Cil.OpCodes.Stloc_2,
			Mono.Cecil.Cil.OpCodes.Stloc_3
		};

		enum ExceptionHandlerState { Try=0, Filter, Catch, Finally, Fault, Done };

		class LocalExceptionHandler
		{
			public ExceptionHandler handler;
			public Label startLabel = new Label();
			public Label endLabel = new Label();

			public ExceptionHandlerState State
			{
				get
				{
					if (handler != null)
						switch (handler.Type)
						{
							case ExceptionHandlerType.Filter:
								return ExceptionHandlerState.Filter;
							case ExceptionHandlerType.Catch:
								return ExceptionHandlerState.Catch;
							case ExceptionHandlerType.Finally:
								return ExceptionHandlerState.Finally;
							case ExceptionHandlerType.Fault:
								return ExceptionHandlerState.Fault;
						}
					return ExceptionHandlerState.Try;
				}
			}
		}

		class ExceptionInfo
		{
			public List<LocalExceptionHandler> handlers = new List<LocalExceptionHandler>();
			public Label endLabel = new Label();
			private bool isDone;

			public ExceptionInfo()
			{
				handlers.Add(new LocalExceptionHandler());
			}

			public ExceptionHandlerState State
			{
				get
				{
					if (isDone)
						return ExceptionHandlerState.Done;

					LocalExceptionHandler lastHandler = handlers[handlers.Count-1];
					return lastHandler.State;
				}
			}

			public void Done()
			{
				Instruction startI = null, endI = null;

				// Set the try start/end instructions
				foreach (LocalExceptionHandler localHandler in handlers)
				{
					if (localHandler.State == ExceptionHandlerState.Try)
					{
						startI = localHandler.startLabel.Instr;
						endI = localHandler.endLabel.Instr;
					}
					else 
						if (localHandler.State == ExceptionHandlerState.Fault ||
							localHandler.State == ExceptionHandlerState.Finally)
					{
						localHandler.handler.TryStart = startI;
						localHandler.handler.TryEnd = localHandler.startLabel.Instr;
					}
					else
					{
						localHandler.handler.TryStart = startI;
						localHandler.handler.TryEnd = endI;
					}
				}

				isDone = true;
			}

			public void Complete()
			{
				// Set all start/end instructions
				LocalExceptionHandler filterHandler = null, voidCatchHandler = null;
				bool markNext = false;

				foreach (LocalExceptionHandler localHandler in handlers)
				{
					if (localHandler.State == ExceptionHandlerState.Try) continue;

					if (markNext)
					{
						voidCatchHandler = localHandler;
						markNext = false;
					}

					if (localHandler.State == ExceptionHandlerState.Filter)
					{
						localHandler.handler.FilterStart = localHandler.startLabel.Instr;
						localHandler.handler.FilterEnd = localHandler.endLabel.Instr;

						// We will delete the next catch handler for empty exception
						markNext = true;
						filterHandler = localHandler;
					}
					else
					{
						localHandler.handler.HandlerStart = localHandler.startLabel.Instr;
						localHandler.handler.HandlerEnd = localHandler.endLabel.Instr;
					}
				}

				if (filterHandler != null)
				{
					if (voidCatchHandler == null)
						throw new System.InvalidOperationException();//Properties.Messages.MissingCatchExceptionBlock);

					// Set Start and End instructions for 'filter' block
					filterHandler.handler.HandlerStart = voidCatchHandler.startLabel.Instr;
					filterHandler.handler.HandlerEnd = voidCatchHandler.endLabel.Instr;

					handlers.Remove(voidCatchHandler);
				}
			}
		}

		internal Mono.Cecil.Cil.CilWorker code;
		internal int localIndex = -1;
		Label markedLabel = null;
		Stack<ExceptionInfo> handlers = new Stack<ExceptionInfo>();
		private Stack<Scope> scopes = new Stack<Scope>();

		internal ILGenerator(Mono.Cecil.Cil.CilWorker code)
		{
			this.code = code;
		}

		internal void Complete()
		{
			if (code == null) return;

			InstructionCollection instrs = code.GetBody().Instructions;

			// Handle labels and local variables
			foreach (Instruction instr in instrs)
				// Handle labels
				if (instr.Operand is Label)
				{
					instr.Operand = ((Label)instr.Operand).Instr;
				}
				else if (instr.Operand is Label[])
				{
					Label[] labels = (Label[]) instr.Operand;
					List<Instruction> linstrs = new List<Instruction>();
					foreach (Label label in labels)
						linstrs.Add(label.Instr);
					instr.Operand = linstrs.ToArray();
				}
				// Handle locals
				else if (instr.Operand is LocalBuilder)
				{
					instr.Operand = ((LocalBuilder) instr.Operand).local;
				}

			// Handle exception handlers
			foreach (ExceptionInfo info in handlers)
				info.Complete();
		}

		private void FinalEmit(Instruction instr)
		{
			if (markedLabel != null)
			{
				markedLabel.Instr = instr;
				markedLabel = null;
			}
		}

		private void MarkPrevious(ExceptionInfo exceptionInfo)
		{
			LocalExceptionHandler previousHandler = exceptionInfo.handlers[exceptionInfo.handlers.Count-1];
			MarkLabel(previousHandler.endLabel);
		}

		private LocalExceptionHandler AddHandler(ExceptionHandlerType handlerType)
		{
			return AddHandler(handlerType, false);
		}
		private LocalExceptionHandler AddHandler(ExceptionHandlerType handlerType, bool afterFilter)
		{
			ExceptionInfo exceptionInfo = handlers.Peek();

			// Mark end label of previous handler
			// End label is only marked by catch or finally blocks
			if (handlerType != ExceptionHandlerType.Fault)
				MarkPrevious(exceptionInfo);

			ExceptionHandler handler = new ExceptionHandler(handlerType);
			if (!afterFilter) code.GetBody().ExceptionHandlers.Add(handler);

			LocalExceptionHandler localHandler = new LocalExceptionHandler();
			localHandler.handler = handler;

			exceptionInfo.handlers.Add(localHandler);
			return localHandler;
		}

		private CallSite BuildCallSite(CallingConvention nativeCallConv, Type returnType, Type[] parameterTypes)
		{
			CallSite site = new CallSite(
					false,
					false,
					CallingConventionConverter.Convert(nativeCallConv),
					new MethodReturnType(returnType.peType));

			foreach (Type paramType in parameterTypes)
				site.Parameters.Add(new ParameterDefinition(paramType.peType));

			return site;
		}
		private CallSite BuildCallSite(System.Reflection.CallingConventions callConvs, Type returnType, Type[] parameterTypes)
		{
			CallSite site = new CallSite(
					(callConvs & System.Reflection.CallingConventions.HasThis) != 0,
					(callConvs & System.Reflection.CallingConventions.ExplicitThis) != 0,
					MethodCallingConvention.Default,
					new MethodReturnType(returnType.peType));

			foreach (Type paramType in parameterTypes)
				site.Parameters.Add(new ParameterDefinition(paramType.peType));

			return site;
		}
#endregion

		/// <summary>
		/// Begins a catch block.  
		/// </summary>
		/// <param name="exceptionType">The <see cref="Type"/> object that represents the exception.</param>
		public void BeginCatchBlock(Type exceptionType)
		{
			ExceptionInfo exceptionInfo = handlers.Count > 0 ? handlers.Peek() : null;
			if (exceptionInfo == null)
			{
				throw new System.NotSupportedException();//Properties.Messages.Argument_NotInExceptionBlock);
			}

			LocalExceptionHandler handler;

			if (exceptionInfo.State == ExceptionHandlerState.Filter)
			{
				if (exceptionType != null)
				{
					throw new System.ArgumentException();//Properties.Messages.Argument_ShouldNotSpecifyExceptionType);
				}

				Emit(OpCodes.Endfilter);

				handler = AddHandler(ExceptionHandlerType.Catch, true);
			}
			else
			{
				if (exceptionType == null)
				{
					throw new System.ArgumentNullException("exceptionType");
				}

				Label endLabel = exceptionInfo.endLabel;
				Emit(OpCodes.Leave, endLabel);

				handler = AddHandler(ExceptionHandlerType.Catch);

				// Set type cached
				handler.handler.CatchType = exceptionType.peType;
			}

			// Mark the start label
			MarkLabel(handler.startLabel);
		}

		/// <summary>
		/// Begins an exception block for a filtered exception. 
		/// </summary>
		public void BeginExceptFilterBlock()
		{
			ExceptionInfo exceptionInfo = handlers.Count > 0 ? handlers.Peek() : null;
			if (exceptionInfo == null)
			{
				throw new System.NotSupportedException();//Properties.Messages.Argument_NotInExceptionBlock);
			}

			Label endLabel = exceptionInfo.endLabel;
			Emit(OpCodes.Leave, endLabel);

			LocalExceptionHandler handler = AddHandler(ExceptionHandlerType.Filter);

			// Mark the start label
			MarkLabel(handler.startLabel);
		}
	
		/// <summary>
		/// Begins an exception block for a non-filtered exception. 
		/// </summary>
		/// <returns>The label for the end of the block. This will leave you in the correct place to execute finally blocks or to finish the try. </returns>
		public Label BeginExceptionBlock()
		{
			ExceptionInfo exceptionInfo;
			handlers.Push(exceptionInfo = new ExceptionInfo());

			// Mark the start label of the 'try' block
			MarkLabel(exceptionInfo.handlers[exceptionInfo.handlers.Count-1].startLabel);

			return exceptionInfo.endLabel;
		}

		/// <summary>
		/// Begins an exception fault block in the Microsoft intermediate language (MSIL) stream. 
		/// </summary>
		public void BeginFaultBlock()
		{
			ExceptionInfo exceptionInfo = handlers.Count > 0 ? handlers.Peek() : null;
			if (exceptionInfo == null)
			{
				throw new System.NotSupportedException();//Properties.Messages.Argument_NotInExceptionBlock);
			}

			Label endLabel = exceptionInfo.endLabel;
			Emit(OpCodes.Leave, endLabel);

			LocalExceptionHandler handler = AddHandler(ExceptionHandlerType.Fault);

			// Mark the start label
			MarkLabel(handler.startLabel);
		}

		/// <summary>
		/// Begins a finally block in the Microsoft intermediate language (MSIL) instruction stream. 
		/// </summary>
		public void BeginFinallyBlock()
		{
			ExceptionInfo exceptionInfo = handlers.Count > 0 ? handlers.Peek() : null;
			if (exceptionInfo == null)
			{
				throw new System.NotSupportedException();//Properties.Messages.Argument_NotInExceptionBlock);
			}

			Label endLabel = exceptionInfo.endLabel;
			if (exceptionInfo.State != ExceptionHandlerState.Try)
			{
				Emit(OpCodes.Leave, endLabel);
			}

			MarkLabel(endLabel);

			LocalExceptionHandler previousHandler = exceptionInfo.handlers[exceptionInfo.handlers.Count-1];
			LocalExceptionHandler tryHandler = null;
			if (previousHandler.State == ExceptionHandlerState.Fault)
			{
				tryHandler = exceptionInfo.handlers[exceptionInfo.handlers.Count - 2];
			}

			LocalExceptionHandler handler = AddHandler(ExceptionHandlerType.Finally);

			Emit(OpCodes.Leave, handler.endLabel);

			if (tryHandler != null)
			{
				// Mark previous 'try' block
				MarkLabel(tryHandler.endLabel);
			}

			// Mark the start label
			MarkLabel(handler.startLabel);
		}
		
		/// <summary>
		/// Begins a lexical scope. 
		/// </summary>
		public void BeginScope()
		{
			Scope scope = new Scope();
			scopes.Push(scope);
			code.GetBody().Scopes.Add(scope);
		}
			
		/// <summary>
		/// Declares a local variable of the specified type. 
		/// </summary>
		/// <param name="localType">A <see cref="Type"/> object that represents the type of the local variable. </param>
		/// <returns>The declared local variable. </returns>
		public LocalBuilder DeclareLocal(Type localType)
		{
			return DeclareLocal(localType, false);
		}

		/// <summary>
		/// Declares a local variable of the specified type, optionally pinning the object referred to by the variable. 
		/// </summary>
		/// <param name="localType">A <see cref="Type"/> object that represents the type of the local variable. </param>
		/// <param name="pinned"><c>true</c> to pin the object in memory; otherwise, <c>false</c>.</param>
		/// <returns>The declared local variable. </returns>
		public LocalBuilder DeclareLocal(Type localType, bool pinned)
		{
		    VariableDefinition local;
			TypeReference vtype = localType.peType;
			if (pinned) vtype = new PinnedType(vtype);

			code.GetBody().Variables.Add(local = new VariableDefinition("V_" + (++localIndex), localIndex, code.GetBody().Method, vtype));
			code.GetBody().InitLocals = true;
			return new LocalBuilder(local);
		}
		
		/// <summary>
		/// Declares a new label. 
		/// </summary>
		/// <returns></returns>
		public Label DefineLabel()
		{
			return new Label();
		}
		
		/// <summary>
		/// Puts the specified instruction onto the stream of instructions. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		public void Emit(OpCode opcode)
		{
			FinalEmit(code.Emit(opcode.op));
		}		

		/// <summary>
		/// Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="arg">The character argument pushed onto the stream immediately after the instruction. </param>
		public void Emit(OpCode opcode, byte arg)
		{
			if (opcode == OpCodes.Ldc_I4_S)
				FinalEmit(code.Emit(opcode.op, (sbyte) arg));
			else
				FinalEmit(code.Emit(opcode.op, arg));
		}
				
		/// <summary>
		/// Puts the specified instruction and metadata token for the specified constructor onto the Microsoft intermediate language (MSIL) stream of instructions. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="ctor">A <see cref="ConstructorInfo"/> representing a constructor. </param>
		public void Emit(OpCode opcode, ConstructorInfo ctor)
		{
			if (ctor is ConstructorInfo.PEConstructorInfo ||
				ctor is ConstructorBuilder)
				FinalEmit(code.Emit(opcode.op, ctor.ctor));
			else if (ctor is ConstructorInfo.PERefConstructorInfo)
				FinalEmit(code.Emit(opcode.op, ((ConstructorInfo.PERefConstructorInfo)ctor).peMeth));
			else
			    FinalEmit(code.Emit(opcode.op, Type.Find(((ConstructorInfo.CLRConstructorInfo) ctor).ctor)));
		}

		/// <summary>
		/// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="arg">The numerical argument pushed onto the stream immediately after the instruction. </param>
		public void Emit(OpCode opcode, double arg)
		{
			FinalEmit(code.Emit(opcode.op, arg));
		}
		
		/// <summary>
		/// Puts the specified instruction and metadata token for the specified field onto the Microsoft intermediate language (MSIL) stream of instructions. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="field">A <see cref="FieldInfo"/> representing a field. </param>
		public void Emit(OpCode opcode, FieldInfo field)
		{
			if (field is FieldInfo.PEFieldInfo ||
				field is FieldBuilder)
				FinalEmit(code.Emit(opcode.op, field.field));
			else if (field is FieldInfo.PERefFieldInfo)
				FinalEmit(code.Emit(opcode.op, ((FieldInfo.PERefFieldInfo)field).field));
			else
				FinalEmit(code.Emit(opcode.op, Type.Find(((FieldInfo.CLRFieldInfo)field).field)));
		}
		
		/// <summary>
		/// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="arg">The <see cref="System.Int16"/> argument pushed onto the stream immediately after the instruction. </param>
		public void Emit(OpCode opcode, System.Int16 arg)
		{
			FinalEmit(code.Emit(opcode.op, arg));
		}
		
		/// <summary>
		/// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="arg">The <see cref="System.Int32"/> argument pushed onto the stream immediately after the instruction. </param>
		public void Emit(OpCode opcode, System.Int32 arg)
		{
			if (opcode == OpCodes.Ldc_I4_S)
				FinalEmit(code.Emit(opcode.op, (sbyte) arg));
			else if (opcode.op.OperandType == OperandType.ShortInlineI ||
					 opcode.op.OperandType == OperandType.ShortInlineParam ||
					 opcode.op.OperandType == OperandType.ShortInlineVar)
				FinalEmit(code.Emit(opcode.op, (byte)arg));
			else
				FinalEmit(code.Emit(opcode.op, arg));
		}
		
		/// <summary>
		/// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="arg">The <see cref="System.Int64"/> argument pushed onto the stream immediately after the instruction. </param>
		public void Emit(OpCode opcode, System.Int64 arg)
		{
		    FinalEmit(code.Emit(opcode.op, arg));
		}
		
		/// <summary>
		/// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="label">The label to which to branch from this location.</param>
		public void Emit(OpCode opcode, Label label)
		{
			Instruction instr;
			if (label.IsValid())
			{
				instr = code.Emit(opcode.op, label.Instr);
			}
			else
			{
				instr = CilWorker.FinalCreate(opcode.op, label);
				code.Append(instr);
			}
			FinalEmit(instr);
		}
		
		/// <summary>
		/// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="labels">The array of label objects to which to branch from this location. All of the labels will be used. </param>
		public void Emit(OpCode opcode, Label[] labels)
		{
			if (Label.AreValid(labels))
			{
				code.Emit(opcode.op, Label.Instrs(labels));
			}
			else
			{
				Instruction instr = CilWorker.FinalCreate(opcode.op, labels);
				code.Append(instr);
				FinalEmit(instr);
			}
		}
	
		/// <summary>
		/// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the index of the given local variable.
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="local">A local variable. </param>
		public void Emit(OpCode opcode, LocalBuilder local)
		{
			if (opcode == OpCodes.Ldloc && local.LocalIndex <= 3)
			{
				FinalEmit(code.Emit(Ldlocs[local.LocalIndex]));
			}
			else if (opcode == OpCodes.Stloc && local.LocalIndex <= 3)
			{
				FinalEmit(code.Emit(Stlocs[local.LocalIndex]));
			}
			else
			{
				FinalEmit(code.Emit(opcode.op, local.local));
			}
		}

		/// <summary>
		/// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given method. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="meth">A <see cref="MethodInfo"/> representing a method. </param>
		public void Emit(OpCode opcode, MethodInfo meth)
		{
			if (meth is MethodInfo.PEMethodInfo ||
				meth is MethodBuilder)
				FinalEmit(code.Emit(opcode.op, meth.meth));
			else if (meth is MethodInfo.PERefMethodInfo)
				FinalEmit(code.Emit(opcode.op, ((MethodInfo.PERefMethodInfo) meth).peMeth));
			else
			    FinalEmit(code.Emit(opcode.op, Type.Find(((MethodInfo.CLRMethodInfo) meth).meth)));
		}

		/// <summary>
		/// Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="arg">The character argument pushed onto the stream immediately after the instruction. </param>
		public void Emit(OpCode opcode, System.SByte arg)
		{
			FinalEmit(code.Emit(opcode.op, arg));
		}
		
		/// <summary>
		/// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="arg">The <see cref="System.Single"/> argument pushed onto the stream immediately after the instruction. </param>
		public void Emit(OpCode opcode, System.Single arg)
		{
			FinalEmit(code.Emit(opcode.op, arg));
		}
		
		/// <summary>
		/// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given string. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="str">The <see cref="System.String"/> to be emitted. </param>
		public void Emit(OpCode opcode, string str)
		{
			FinalEmit(code.Emit(opcode.op, str));
		}
	
		/// <summary>
		/// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given type.
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="cls">A <see cref="Type"/>.</param>
		public void Emit(OpCode opcode, Type cls)
		{
			FinalEmit(code.Emit(opcode.op, cls.peType));
		}
		
		/// <summary>
		/// Puts a call or callvirt instruction onto the Microsoft intermediate language (MSIL) stream. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="methodInfo">The method to be called. </param>
		/// <param name="optionalParameterTypes">The types of the optional arguments if the method is a varargs method; otherwise, a <c>null</c> reference (<c>Nothing</c> in Visual Basic).</param>
		public void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			Emit(opcode, methodInfo);
		}
		
		/// <summary>
		/// Puts a Calli instruction onto the Microsoft intermediate language (MSIL) stream, specifying an unmanaged calling convention for the indirect call. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="unmanagedCallConv">The unmanaged calling convention to be used. </param>
		/// <param name="returnType">The <see cref="Type"/> of the result. </param>
		/// <param name="parameterTypes">The types of the required arguments to the instruction.</param>
		public void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			Instruction instr = CilWorker.FinalCreate(Mono.Cecil.Cil.OpCodes.Calli, BuildCallSite(unmanagedCallConv, returnType, parameterTypes));
			code.Append(instr);
			FinalEmit(instr);
		}		

		/// <summary>
		/// Puts a Calli instruction onto the Microsoft intermediate language (MSIL) stream, specifying a managed calling convention for the indirect call. 
		/// </summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		/// <param name="callingConvention">The managed calling convention to be used. </param>
		/// <param name="returnType">The <see cref="Type"/> of the result. </param>
		/// <param name="parameterTypes">The types of the required arguments to the instruction.</param>
		/// <param name="optionalParameterTypes">The types of the optional arguments if the method is a varargs method; otherwise, a <c>null</c> reference (<c>Nothing</c> in Visual Basic).</param>
		public void EmitCalli(OpCode opcode, System.Reflection.CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			Instruction instr = CilWorker.FinalCreate(Mono.Cecil.Cil.OpCodes.Calli, BuildCallSite(callingConvention, returnType, parameterTypes));
			code.Append(instr);
			FinalEmit(instr);
		}
		
		/// <summary>
		/// Emits the Microsoft intermediate language (MSIL) necessary to call <c>System.Console.WriteLine</c> with the given field. 
		/// </summary>
		/// <param name="fld">The field whose value is to be written to the console. </param>
		public void EmitWriteLine(FieldInfo fld)
		{
			// Emit "System.Console.Out.WriteLine(<obj>.<field>);"
			MethodInfo method = typeof(System.Console).GetMethod("get_Out");
			Emit(OpCodes.Call, method);

			if ((fld.Attributes & System.Reflection.FieldAttributes.Static) != System.Reflection.FieldAttributes.PrivateScope)
			{
				Emit(OpCodes.Ldsfld, fld);
			}
			else
			{
				Emit(OpCodes.Ldarg, (short) 0);
				Emit(OpCodes.Ldfld, fld);
			}
			
			Type[] types = new Type[1];
			object fieldType = fld.FieldType;
			if ((fieldType is TypeBuilder) || (fieldType is EnumBuilder))
			{
				throw new System.NotSupportedException();//Properties.Messages.NotSupported_OutputStreamUsingTypeBuilder);
			}
			types[0] = (Type) fieldType;
			MethodInfo meth = typeof(System.IO.TextWriter).GetMethod("WriteLine", Type.Unwrap(types));
			if (meth == null)
			{
				//throw new ArgumentException(Properties.Messages.Argument_EmitWriteLineType, "fld");
			}
			Emit(OpCodes.Callvirt, meth);
		}
		
		/// <summary>
		/// Emits the Microsoft intermediate language (MSIL) necessary to call <c>System.Console.WriteLine</c> with the given local variable. 
		/// </summary>
		/// <param name="localBuilder">The local variable whose value is to be written to the console. </param>
		public void EmitWriteLine(LocalBuilder localBuilder)
		{
			// Emit "System.Console.Out.WriteLine(<local>);"
			MethodInfo method = typeof(System.Console).GetMethod("get_Out");
			Emit(OpCodes.Call, method);
			Emit(OpCodes.Ldloc, localBuilder);
			
			Type[] types = new Type[1];
			object localType = localBuilder.LocalType;
			if ((localType is TypeBuilder) || (localType is EnumBuilder))
			{
				//throw new ArgumentException(Properties.Messages.NotSupported_OutputStreamUsingTypeBuilder);
			}
			types[0] = (Type) localType;
			MethodInfo meth = typeof(System.IO.TextWriter).GetMethod("WriteLine", Type.Unwrap(types));
			if (meth == null)
			{
				//throw new ArgumentException(Properties.Messages.Argument_EmitWriteLineType, "localBuilder");
			}
			Emit(OpCodes.Callvirt, meth);
		}
			
		/// <summary>
		/// Emits the Microsoft intermediate language (MSIL) to call <c>System.Console.WriteLine</c> with a string. 
		/// </summary>
		/// <param name="value">The string to be printed. </param>
		public void EmitWriteLine(string value)
		{
			// Emit "System.Console.WriteLine(value);"
			Emit(OpCodes.Ldstr, value);
			MethodInfo method = typeof(System.Console).GetMethod("WriteLine", new System.Type[] { typeof(string) });
			Emit(OpCodes.Call, method);
		}

		/// <summary>
		/// Ends an exception block. 
		/// </summary>
		public void EndExceptionBlock()
		{
			ExceptionInfo exceptionInfo = handlers.Count > 0 ? handlers.Peek() : null;
			if (exceptionInfo == null)
			{
				throw new System.NotSupportedException();//Properties.Messages.Argument_NotInExceptionBlock);
			}

			Label endLabel = exceptionInfo.endLabel;
			switch (exceptionInfo.State)
			{
				case ExceptionHandlerState.Try:
				case ExceptionHandlerState.Filter:
					throw new System.InvalidOperationException();//Properties.Messages.Argument_BadExceptionCodeGen);

				case ExceptionHandlerState.Catch:
					Emit(OpCodes.Leave, endLabel);
					break;

				case ExceptionHandlerState.Fault:
				case ExceptionHandlerState.Finally:
					Emit(OpCodes.Endfinally);
					break;
			}

			// Mark end label of previous handler
			MarkPrevious(exceptionInfo);

			if (!endLabel.isMarked) 
				MarkLabel(endLabel);

			exceptionInfo.Done();
		}
		
		/// <summary>
		/// Ends a lexical scope. 
		/// </summary>
		public void EndScope()
		{
			Scope scope = scopes.Pop();
			code.GetBody().Scopes.Remove(scope);
		}

		/// <summary>
		/// Marks the Microsoft intermediate language (MSIL) stream's current position with the given label. 
		/// </summary>
		/// <param name="loc">The label for which to set an index. </param>
		public void MarkLabel(Label loc)
		{
			if (markedLabel != null) markedLabel.linked = loc;
			markedLabel = loc;
			loc.isMarked = true;
		}
		
		/// <summary>
		/// Emits an instruction to throw an exception. 
		/// </summary>
		/// <param name="excType">The class of the type of exception to throw.</param>
		public void ThrowException(Type excType)
		{
			if (!excType.IsSubclassOf(Type.ExceptionType) && (excType.peType != Type.ExceptionType))
			{
				//throw new System.ArgumentException(Properties.Messages.Argument_NotExceptionType);
			}
			ConstructorInfo constructor = excType.GetConstructor(Type.EmptyTypes);
			if (constructor == null)
			{
				//throw new System.ArgumentException(Properties.Messages.Argument_MissingDefaultConstructor);
			}
			this.Emit(OpCodes.Newobj, constructor);
			this.Emit(OpCodes.Throw);
		}

		/// <summary>
		/// Marks a sequence point in the Microsoft intermediate language (MSIL) stream. 
		/// </summary>
		/// <param name="document">The document for which the sequence point is being defined. </param>
		/// <param name="startLine">The line where the sequence point begins. </param>
		/// <param name="startColumn">The column in the line where the sequence point begins. </param>
		/// <param name="endLine">The line where the sequence point ends. </param>
		/// <param name="endColumn">The column in the line where the sequence point ends. </param>
		public void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Specifies the namespace to be used in evaluating locals and watches for the current active lexical scope. 
		/// </summary>
		/// <param name="usingNamespace">he namespace to be used in evaluating locals and watches for the current active lexical scope </param>
		public void UsingNamespace(string usingNamespace)
		{
			throw new System.NotImplementedException();
		}

  	}
}
