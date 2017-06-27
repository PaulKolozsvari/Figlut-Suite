using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Provides field representations of the Microsoft Intermediate Language (MSIL) instructions for emission by the <see cref="ILGenerator"/> class members (such as <see cref="Emit"/>). 
	/// </summary>
	public class OpCodes
	{
		// Fields
		/// <summary>
		/// Fills space if opcodes are patched. No meaningful operation is performed although a processing cycle can be consumed. 
		/// </summary>
        public static OpCode Nop = Mono.Cecil.Cil.OpCodes.Nop;
		/// <summary>
		/// Signals the Common Language Infrastructure (CLI) to inform the debugger that a break point has been tripped. 
		/// </summary>
		public static OpCode Break = Mono.Cecil.Cil.OpCodes.Break;
		/// <summary>
		/// Loads the argument at index 0 onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldarg_0 = Mono.Cecil.Cil.OpCodes.Ldarg_0;
		/// <summary>
		/// Loads the argument at index 1 onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldarg_1 = Mono.Cecil.Cil.OpCodes.Ldarg_1;
		/// <summary>
		/// Loads the argument at index 2 onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldarg_2 = Mono.Cecil.Cil.OpCodes.Ldarg_2;
		/// <summary>
		/// Loads the argument at index 3 onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldarg_3 = Mono.Cecil.Cil.OpCodes.Ldarg_3;
		/// <summary>
		/// Loads the local variable at index 0 onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldloc_0 = Mono.Cecil.Cil.OpCodes.Ldloc_0;
		/// <summary>
		/// Loads the local variable at index 1 onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldloc_1 = Mono.Cecil.Cil.OpCodes.Ldloc_1;
		/// <summary>
		/// Loads the local variable at index 2 onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldloc_2 = Mono.Cecil.Cil.OpCodes.Ldloc_2;
		/// <summary>
		/// Loads the local variable at index 3 onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldloc_3 = Mono.Cecil.Cil.OpCodes.Ldloc_3;
		/// <summary>
		/// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 0. 
		/// </summary>
		public static OpCode Stloc_0 = Mono.Cecil.Cil.OpCodes.Stloc_0;
		/// <summary>
		/// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 1. 
		/// </summary>
		public static OpCode Stloc_1 = Mono.Cecil.Cil.OpCodes.Stloc_1;
		/// <summary>
		/// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 2. 
		/// </summary>
		public static OpCode Stloc_2 = Mono.Cecil.Cil.OpCodes.Stloc_2;
		/// <summary>
		/// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 3. 
		/// </summary>
		public static OpCode Stloc_3 = Mono.Cecil.Cil.OpCodes.Stloc_3;
		/// <summary>
		/// Loads the argument (referenced by a specified short form index) onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldarg_S = Mono.Cecil.Cil.OpCodes.Ldarg_S;
		/// <summary>
		/// Load an argument address, in short form, onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldarga_S = Mono.Cecil.Cil.OpCodes.Ldarga_S;
		/// <summary>
		/// Stores the value on top of the evaluation stack in the argument slot at a specified index, short form. 
		/// </summary>
		public static OpCode Starg_S = Mono.Cecil.Cil.OpCodes.Starg_S;
		/// <summary>
		/// Loads the local variable at a specific index onto the evaluation stack, short form. 
		/// </summary>
		public static OpCode Ldloc_S = Mono.Cecil.Cil.OpCodes.Ldloc_S;
		/// <summary>
		/// Loads the address of the local variable at a specific index onto the evaluation stack, short form. 
		/// </summary>
		public static OpCode Ldloca_S = Mono.Cecil.Cil.OpCodes.Ldloca_S;
		/// <summary>
		/// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index (short form). 
		/// </summary>
		public static OpCode Stloc_S = Mono.Cecil.Cil.OpCodes.Stloc_S;
		/// <summary>Pushes a null reference (type <c>O</c>) onto the evaluation stack. </summary>
		public static OpCode Ldnull = Mono.Cecil.Cil.OpCodes.Ldnull;
		/// <summary>
		/// Pushes the integer value of -1 onto the evaluation stack as an <see cref="System.Int32"/>. 
		/// </summary>
		public static OpCode Ldc_I4_M1 = Mono.Cecil.Cil.OpCodes.Ldc_I4_M1;
		/// <summary>
		/// Pushes the integer value of 0 onto the evaluation stack as an <see cref="System.Int32"/>. 
		/// </summary>
		public static OpCode Ldc_I4_0 = Mono.Cecil.Cil.OpCodes.Ldc_I4_0;
		/// <summary>
		/// Pushes the integer value of 1 onto the evaluation stack as an <see cref="System.Int32"/>. 
		/// </summary>
		public static OpCode Ldc_I4_1 = Mono.Cecil.Cil.OpCodes.Ldc_I4_1;
		/// <summary>
		/// Pushes the integer value of 2 onto the evaluation stack as an <see cref="System.Int32"/>. 
		/// </summary>
		public static OpCode Ldc_I4_2 = Mono.Cecil.Cil.OpCodes.Ldc_I4_2;
		/// <summary>
		/// Pushes the integer value of 3 onto the evaluation stack as an <see cref="System.Int32"/>. 
		/// </summary>
		public static OpCode Ldc_I4_3 = Mono.Cecil.Cil.OpCodes.Ldc_I4_3;
		/// <summary>
		/// Pushes the integer value of 4 onto the evaluation stack as an <see cref="System.Int32"/>. 
		/// </summary>
		public static OpCode Ldc_I4_4 = Mono.Cecil.Cil.OpCodes.Ldc_I4_4;
		/// <summary>
		/// Pushes the integer value of 5 onto the evaluation stack as an <see cref="System.Int32"/>. 
		/// </summary>
		public static OpCode Ldc_I4_5 = Mono.Cecil.Cil.OpCodes.Ldc_I4_5;
		/// <summary>
		/// Pushes the integer value of 6 onto the evaluation stack as an <see cref="System.Int32"/>. 
		/// </summary>
		public static OpCode Ldc_I4_6 = Mono.Cecil.Cil.OpCodes.Ldc_I4_6;
		/// <summary>
		/// Pushes the integer value of 7 onto the evaluation stack as an <see cref="System.Int32"/>. 
		/// </summary>
		public static OpCode Ldc_I4_7 = Mono.Cecil.Cil.OpCodes.Ldc_I4_7;
		/// <summary>
		/// Pushes the integer value of 8 onto the evaluation stack as an <see cref="System.Int32"/>. 
		/// </summary>
		public static OpCode Ldc_I4_8 = Mono.Cecil.Cil.OpCodes.Ldc_I4_8;
		/// <summary>
		/// Pushes the supplied <see cref="System.Int16"/> value onto the evaluation stack as an <see cref="System.Int32"/>, short form. 
		/// </summary>
		public static OpCode Ldc_I4_S = Mono.Cecil.Cil.OpCodes.Ldc_I4_S;
		/// <summary>
		/// Pushes the supplied <see cref="System.Int32"/> value onto the evaluation stack as an <see cref="System.Int32"/>. 
		/// </summary>
		public static OpCode Ldc_I4 = Mono.Cecil.Cil.OpCodes.Ldc_I4;
		/// <summary>
		/// Pushes the supplied <see cref="System.Int64"/> value onto the evaluation stack as an <see cref="System.Int64"/>. 
		/// </summary>
		public static OpCode Ldc_I8 = Mono.Cecil.Cil.OpCodes.Ldc_I8;
		/// <summary>
		/// Pushes a supplied value of type <see cref="System.Single"/> onto the evaluation stack as type F (float). 
		/// </summary>
		public static OpCode Ldc_R4 = Mono.Cecil.Cil.OpCodes.Ldc_R4;
		/// <summary>
		/// Pushes a supplied value of type <see cref="System.Double"/> onto the evaluation stack as type F (float). 
		/// </summary>
		public static OpCode Ldc_R8 = Mono.Cecil.Cil.OpCodes.Ldc_R8;
		/// <summary>
		/// Copies the current topmost value on the evaluation stack, and then pushes the copy onto the evaluation stack. 
		/// </summary>
		public static OpCode Dup = Mono.Cecil.Cil.OpCodes.Dup;
		/// <summary>
		/// Removes the value currently on top of the evaluation stack. 
		/// </summary>
		public static OpCode Pop = Mono.Cecil.Cil.OpCodes.Pop;
		/// <summary>
		/// Exits current method and jumps to specified method. 
		/// </summary>
		public static OpCode Jmp = Mono.Cecil.Cil.OpCodes.Jmp;
		/// <summary>
		/// Calls the method indicated by the passed method descriptor. 
		/// </summary>
		public static OpCode Call = Mono.Cecil.Cil.OpCodes.Call;
		/// <summary>
		/// Calls the method indicated on the evaluation stack (as a pointer to an entry point) with arguments described by a calling convention.
		/// </summary>
		public static OpCode Calli = Mono.Cecil.Cil.OpCodes.Calli;
		/// <summary>
		/// Returns from the current method, pushing a return value (if present) from the callee's evaluation stack onto the caller's evaluation stack. 
		/// </summary>
		public static OpCode Ret = Mono.Cecil.Cil.OpCodes.Ret;
		/// <summary>
		/// Unconditionally transfers control to a target instruction (short form). 
		/// </summary>
		public static OpCode Br_S = Mono.Cecil.Cil.OpCodes.Br_S;
		/// <summary>
		/// Transfers control to a target instruction if value is false, a null reference, or zero. 
		/// </summary>
		public static OpCode Brfalse_S = Mono.Cecil.Cil.OpCodes.Brfalse_S;
		/// <summary>
		/// Transfers control to a target instruction (short form) if value is true, not null, or non-zero. 
		/// </summary>
		public static OpCode Brtrue_S = Mono.Cecil.Cil.OpCodes.Brtrue_S;
		/// <summary>
		/// Transfers control to a target instruction (short form) if two values are equal. 
		/// </summary>
		public static OpCode Beq_S = Mono.Cecil.Cil.OpCodes.Beq_S;
		/// <summary>
		/// Transfers control to a target instruction (short form) if the first value is greater than or equal to the second value. 
		/// </summary>
		public static OpCode Bge_S = Mono.Cecil.Cil.OpCodes.Bge_S;
		/// <summary>
		/// Transfers control to a target instruction (short form) if the first value is greater than the second value. 
		/// </summary>
		public static OpCode Bgt_S = Mono.Cecil.Cil.OpCodes.Bgt_S;
		/// <summary>
		/// Transfers control to a target instruction (short form) if the first value is less than or equal to the second value. 
		/// </summary>
		public static OpCode Ble_S = Mono.Cecil.Cil.OpCodes.Ble_S;
		/// <summary>
		/// Transfers control to a target instruction (short form) if the first value is less than the second value. 
		/// </summary>
		public static OpCode Blt_S = Mono.Cecil.Cil.OpCodes.Blt_S;
		/// <summary>
		/// Transfers control to a target instruction (short form) when two unsigned integer values or unordered float values are not equal. 
		/// </summary>
		public static OpCode Bne_Un_S = Mono.Cecil.Cil.OpCodes.Bne_Un_S;
		/// <summary>
		/// Transfers control to a target instruction (short form) if the first value is greater than the second value, when comparing unsigned integer values or unordered float values. 
		/// </summary>
		public static OpCode Bge_Un_S = Mono.Cecil.Cil.OpCodes.Bge_Un_S;
		/// <summary>
		/// Transfers control to a target instruction (short form) if the first value is greater than the second value, when comparing unsigned integer values or unordered float values. 
		/// </summary>
		public static OpCode Bgt_Un_S = Mono.Cecil.Cil.OpCodes.Bgt_Un_S;
		/// <summary>
		/// Transfers control to a target instruction (short form) if the first value is less than or equal to the second value, when comparing unsigned integer values or unordered float values. 
		/// </summary>
		public static OpCode Ble_Un_S = Mono.Cecil.Cil.OpCodes.Ble_Un_S;
		/// <summary>
		/// Transfers control to a target instruction (short form) if the first value is less than the second value, when comparing unsigned integer values or unordered float values. 
		/// </summary>
		public static OpCode Blt_Un_S = Mono.Cecil.Cil.OpCodes.Blt_Un_S;
		/// <summary>
		/// Unconditionally transfers control to a target instruction. 
		/// </summary>
		public static OpCode Br = Mono.Cecil.Cil.OpCodes.Br;
		/// <summary>
		/// Transfers control to a target instruction if value is false, a null reference (Nothing in Visual Basic), or zero. 
		/// </summary>
		public static OpCode Brfalse = Mono.Cecil.Cil.OpCodes.Brfalse;
		/// <summary>
		/// Transfers control to a target instruction if value is true, not null, or non-zero. 
		/// </summary>
		public static OpCode Brtrue = Mono.Cecil.Cil.OpCodes.Brtrue;
		/// <summary>
		/// Transfers control to a target instruction if two values are equal. 
		/// </summary>
		public static OpCode Beq = Mono.Cecil.Cil.OpCodes.Beq;
		/// <summary>
		/// Transfers control to a target instruction if the first value is greater than or equal to the second value. 
		/// </summary>
		public static OpCode Bge = Mono.Cecil.Cil.OpCodes.Bge;
		/// <summary>
		/// Transfers control to a target instruction if the first value is greater than the second value. 
		/// </summary>
		public static OpCode Bgt = Mono.Cecil.Cil.OpCodes.Bgt;
		/// <summary>
		/// Transfers control to a target instruction if the first value is less than or equal to the second value. 
		/// </summary>
		public static OpCode Ble = Mono.Cecil.Cil.OpCodes.Ble;
		/// <summary>
		/// Transfers control to a target instruction if the first value is less than the second value. 
		/// </summary>
		public static OpCode Blt = Mono.Cecil.Cil.OpCodes.Blt;
		/// <summary>
		/// Transfers control to a target instruction when two unsigned integer values or unordered float values are not equal.
		/// </summary>
		public static OpCode Bne_Un = Mono.Cecil.Cil.OpCodes.Bne_Un;
		/// <summary>
		/// Transfers control to a target instruction if the first value is greater than the second value, when comparing unsigned integer values or unordered float values. 
		/// </summary>
		public static OpCode Bge_Un = Mono.Cecil.Cil.OpCodes.Bge_Un;
		/// <summary>
		/// Transfers control to a target instruction if the first value is greater than the second value, when comparing unsigned integer values or unordered float values. 
		/// </summary>
		public static OpCode Bgt_Un = Mono.Cecil.Cil.OpCodes.Bgt_Un;
		/// <summary>
		/// Transfers control to a target instruction if the first value is less than or equal to the second value, when comparing unsigned integer values or unordered float values. 
		/// </summary>
		public static OpCode Ble_Un = Mono.Cecil.Cil.OpCodes.Ble_Un;
		/// <summary>
		/// Transfers control to a target instruction if the first value is less than the second value, when comparing unsigned integer values or unordered float values. 
		/// </summary>
		public static OpCode Blt_Un = Mono.Cecil.Cil.OpCodes.Blt_Un;
		/// <summary>
		/// Implements a jump table.
		/// </summary>
		public static OpCode Switch = Mono.Cecil.Cil.OpCodes.Switch;
		/// <summary>
		/// Loads a value of type int8 as an int32 onto the evaluation stack indirectly. 
		/// </summary>
		public static OpCode Ldind_I1 = Mono.Cecil.Cil.OpCodes.Ldind_I1;
		/// <summary>
		/// Loads a value of type unsigned int8 as an int32 onto the evaluation stack indirectly. 
		/// </summary>
		public static OpCode Ldind_U1 = Mono.Cecil.Cil.OpCodes.Ldind_U1;
		/// <summary>
		/// Loads a value of type int16 as an int32 onto the evaluation stack indirectly. 
		/// </summary>
		public static OpCode Ldind_I2 = Mono.Cecil.Cil.OpCodes.Ldind_I2;
		/// <summary>
		/// Loads a value of type unsigned int16 as an int32 onto the evaluation stack indirectly. 
		/// </summary>
		public static OpCode Ldind_U2 = Mono.Cecil.Cil.OpCodes.Ldind_U2;
		/// <summary>
		/// Loads a value of type int32 as an int32 onto the evaluation stack indirectly. 
		/// </summary>
		public static OpCode Ldind_I4 = Mono.Cecil.Cil.OpCodes.Ldind_I4;
		/// <summary>
		/// Loads a value of type unsigned int32 as an int32 onto the evaluation stack indirectly. 
		/// </summary>
		public static OpCode Ldind_U4 = Mono.Cecil.Cil.OpCodes.Ldind_U4;
		/// <summary>
		/// Loads a value of type int64 as an int64 onto the evaluation stack indirectly. 
		/// </summary>
		public static OpCode Ldind_I8 = Mono.Cecil.Cil.OpCodes.Ldind_I8;
		/// <summary>
		/// Loads a value of type natural int as a natural int onto the evaluation stack indirectly. 
		/// </summary>
		public static OpCode Ldind_I = Mono.Cecil.Cil.OpCodes.Ldind_I;
		/// <summary>
		/// Loads a value of type float32 as a type F (float) onto the evaluation stack indirectly. 
		/// </summary>
		public static OpCode Ldind_R4 = Mono.Cecil.Cil.OpCodes.Ldind_R4;
		/// <summary>
		/// Loads a value of type float64 as a type F (float) onto the evaluation stack indirectly. 
		/// </summary>
		public static OpCode Ldind_R8 = Mono.Cecil.Cil.OpCodes.Ldind_R8;
		/// <summary>
		/// Loads an object reference as a type O (object reference) onto the evaluation stack indirectly. 
		/// </summary>
		public static OpCode Ldind_Ref = Mono.Cecil.Cil.OpCodes.Ldind_Ref;
		/// <summary>
		/// Stores a object reference value at a supplied address. 
		/// </summary>
		public static OpCode Stind_Ref = Mono.Cecil.Cil.OpCodes.Stind_Ref;
		/// <summary>
		/// Stores a value of type int8 at a supplied address. 
		/// </summary>
		public static OpCode Stind_I1 = Mono.Cecil.Cil.OpCodes.Stind_I1;
		/// <summary>
		/// Stores a value of type int16 at a supplied address. 
		/// </summary>
		public static OpCode Stind_I2 = Mono.Cecil.Cil.OpCodes.Stind_I2;
		/// <summary>
		/// Stores a value of type int32 at a supplied address. 
		/// </summary>
		public static OpCode Stind_I4 = Mono.Cecil.Cil.OpCodes.Stind_I4;
		/// <summary>
		/// Stores a value of type int64 at a supplied address. 
		/// </summary>
		public static OpCode Stind_I8 = Mono.Cecil.Cil.OpCodes.Stind_I8;
		/// <summary>
		/// Stores a value of type float32 at a supplied address. 
		/// </summary>
		public static OpCode Stind_R4 = Mono.Cecil.Cil.OpCodes.Stind_R4;
		/// <summary>
		/// Stores a value of type float64 at a supplied address. 
		/// </summary>
		public static OpCode Stind_R8 = Mono.Cecil.Cil.OpCodes.Stind_R8;
		/// <summary>
		/// Adds two values and pushes the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Add = Mono.Cecil.Cil.OpCodes.Add;
		/// <summary>
		/// Subtracts one value from another and pushes the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Sub = Mono.Cecil.Cil.OpCodes.Sub;
		/// <summary>
		/// Multiplies two values and pushes the result on the evaluation stack. 
		/// </summary>
		public static OpCode Mul = Mono.Cecil.Cil.OpCodes.Mul;
		/// <summary>
		/// Divides two values and pushes the result as a floating-point (type F) or quotient (type int32) onto the evaluation stack. 
		/// </summary>
		public static OpCode Div = Mono.Cecil.Cil.OpCodes.Div;
		/// <summary>
		/// Divides two unsigned integer values and pushes the result (int32) onto the evaluation stack. 
		/// </summary>
		public static OpCode Div_Un = Mono.Cecil.Cil.OpCodes.Div_Un;
		/// <summary>
		/// Divides two values and pushes the remainder onto the evaluation stack. 
		/// </summary>
		public static OpCode Rem = Mono.Cecil.Cil.OpCodes.Rem;
		/// <summary>
		/// Divides two unsigned values and pushes the remainder onto the evaluation stack. 
		/// </summary>
		public static OpCode Rem_Un = Mono.Cecil.Cil.OpCodes.Rem_Un;
		/// <summary>
		/// Computes the bitwise AND of two values and pushes the result onto the evaluation stack. 
		/// </summary>
		public static OpCode And = Mono.Cecil.Cil.OpCodes.And;
		/// <summary>
		/// Compute the bitwise complement of the two integer values on top of the stack and pushes the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Or = Mono.Cecil.Cil.OpCodes.Or;
		/// <summary>
		/// Computes the bitwise XOR of the top two values on the evaluation stack, pushing the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Xor = Mono.Cecil.Cil.OpCodes.Xor;
		/// <summary>
		/// Shifts an integer value to the left (in zeroes) by a specified number of bits, pushing the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Shl = Mono.Cecil.Cil.OpCodes.Shl;
		/// <summary>
		/// Shifts an integer value (in sign) to the right by a specified number of bits, pushing the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Shr = Mono.Cecil.Cil.OpCodes.Shr;
		/// <summary>
		/// Shifts an unsigned integer value (in zeroes) to the right by a specified number of bits, pushing the result onto the evaluation stack.
		/// </summary>
		public static OpCode Shr_Un = Mono.Cecil.Cil.OpCodes.Shr_Un;
		/// <summary>
		/// Negates a value and pushes the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Neg = Mono.Cecil.Cil.OpCodes.Neg;
		/// <summary>
		/// Computes the bitwise complement of the integer value on top of the stack and pushes the result onto the evaluation stack as the same type. 
		/// </summary>
		public static OpCode Not = Mono.Cecil.Cil.OpCodes.Not;
		/// <summary>
		/// Converts the value on top of the evaluation stack to int8, then extends (pads) it to int32. 
		/// </summary>
		public static OpCode Conv_I1 = Mono.Cecil.Cil.OpCodes.Conv_I1;
		/// <summary>
		/// Converts the value on top of the evaluation stack to int16, then extends (pads) it to int32. 
		/// </summary>
		public static OpCode Conv_I2 = Mono.Cecil.Cil.OpCodes.Conv_I2;
		/// <summary>
		/// Converts the value on top of the evaluation stack to int32. 
		/// </summary>
		public static OpCode Conv_I4 = Mono.Cecil.Cil.OpCodes.Conv_I4;
		/// <summary>
		/// Converts the value on top of the evaluation stack to int64. 
		/// </summary>
		public static OpCode Conv_I8 = Mono.Cecil.Cil.OpCodes.Conv_I8;
		/// <summary>
		/// Converts the value on top of the evaluation stack to float32. 
		/// </summary>
		public static OpCode Conv_R4 = Mono.Cecil.Cil.OpCodes.Conv_R4;
		/// <summary>
		/// Converts the value on top of the evaluation stack to float64. 
		/// </summary>
		public static OpCode Conv_R8 = Mono.Cecil.Cil.OpCodes.Conv_R8;
		/// <summary>
		/// Converts the value on top of the evaluation stack to unsigned int32, and extends it to int32. 
		/// </summary>
		public static OpCode Conv_U4 = Mono.Cecil.Cil.OpCodes.Conv_U4;
		/// <summary>
		/// Converts the value on top of the evaluation stack to unsigned int64, and extends it to int64. 
		/// </summary>
		public static OpCode Conv_U8 = Mono.Cecil.Cil.OpCodes.Conv_U8;
		/// <summary>
		/// Calls a late-bound method on an object, pushing the return value onto the evaluation stack.
		/// </summary>
		public static OpCode Callvirt = Mono.Cecil.Cil.OpCodes.Callvirt;
		/// <summary>
		/// Copies the value type located at the address of an object (type &amp;, * or natural int) to the address of the destination object (type &amp;, * or natural int). 
		/// </summary>
		public static OpCode Cpobj = Mono.Cecil.Cil.OpCodes.Cpobj;
		/// <summary>
		/// Copies the value type object pointed to by an address to the top of the evaluation stack. 
		/// </summary>
		public static OpCode Ldobj = Mono.Cecil.Cil.OpCodes.Ldobj;
		/// <summary>
		/// Pushes a new object reference to a string literal stored in the metadata. 
		/// </summary>
		public static OpCode Ldstr = Mono.Cecil.Cil.OpCodes.Ldstr;
		/// <summary>
		/// Creates a new object or a new instance of a value type, pushing an object reference (type O) onto the evaluation stack.
		/// </summary>
		public static OpCode Newobj = Mono.Cecil.Cil.OpCodes.Newobj;
		/// <summary>
		/// Attempts to cast an object passed by reference to the specified class. 
		/// </summary>
		public static OpCode Castclass = Mono.Cecil.Cil.OpCodes.Castclass;
		/// <summary>
		/// Tests whether an object reference (type O) is an instance of a particular class. 
		/// </summary>
		public static OpCode Isinst = Mono.Cecil.Cil.OpCodes.Isinst;
		/// <summary>
		/// Converts the unsigned integer value on top of the evaluation stack to float32. 
		/// </summary>
		public static OpCode Conv_R_Un = Mono.Cecil.Cil.OpCodes.Conv_R_Un;
		/// <summary>
		/// Converts the boxed representation of a value type to its unboxed form. 
		/// </summary>
		public static OpCode Unbox = Mono.Cecil.Cil.OpCodes.Unbox;
		/// <summary>
		/// Throws the exception object currently on the evaluation stack. 
		/// </summary>
		public static OpCode Throw = Mono.Cecil.Cil.OpCodes.Throw;
		/// <summary>
		/// Finds the value of a field in the object whose reference is currently on the evaluation stack. 
		/// </summary>
		public static OpCode Ldfld = Mono.Cecil.Cil.OpCodes.Ldfld;
		/// <summary>
		/// Finds the address of a field in the object whose reference is currently on the evaluation stack. 
		/// </summary>
		public static OpCode Ldflda = Mono.Cecil.Cil.OpCodes.Ldflda;
		/// <summary>
		/// Replaces the value stored in the field of an object reference or pointer with a new value.
		/// </summary>
		public static OpCode Stfld = Mono.Cecil.Cil.OpCodes.Stfld;
		/// <summary>
		/// Pushes the value of a static field onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldsfld = Mono.Cecil.Cil.OpCodes.Ldsfld;
		/// <summary>
		/// Pushes the address of a static field onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldsflda = Mono.Cecil.Cil.OpCodes.Ldsflda;
		/// <summary>
		/// Replaces the value of a static field with a value from the evaluation stack. 
		/// </summary>
		public static OpCode Stsfld = Mono.Cecil.Cil.OpCodes.Stsfld;
		/// <summary>
		/// Copies a value of a specified type from the evaluation stack into a supplied memory address. 
		/// </summary>
		public static OpCode Stobj = Mono.Cecil.Cil.OpCodes.Stobj;
		/// <summary>
		/// Converts the unsigned value on top of the evaluation stack to signed int8 and extends it to int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_I1_Un = Mono.Cecil.Cil.OpCodes.Conv_Ovf_I1_Un;
		/// <summary>
		/// Converts the unsigned value on top of the evaluation stack to signed int16 and extends it to int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_I2_Un = Mono.Cecil.Cil.OpCodes.Conv_Ovf_I2_Un;
		/// <summary>
		/// Converts the unsigned value on top of the evaluation stack to signed int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_I4_Un = Mono.Cecil.Cil.OpCodes.Conv_Ovf_I4_Un;
		/// <summary>
		/// Converts the unsigned value on top of the evaluation stack to signed int64, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_I8_Un = Mono.Cecil.Cil.OpCodes.Conv_Ovf_I8_Un;
		/// <summary>
		/// Converts the unsigned value on top of the evaluation stack to unsigned int8 and extends it to int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_U1_Un = Mono.Cecil.Cil.OpCodes.Conv_Ovf_U1_Un;
		/// <summary>
		/// Converts the unsigned value on top of the evaluation stack to unsigned int16 and extends it to int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_U2_Un = Mono.Cecil.Cil.OpCodes.Conv_Ovf_U2_Un;
		/// <summary>
		/// Converts the unsigned value on top of the evaluation stack to unsigned int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_U4_Un = Mono.Cecil.Cil.OpCodes.Conv_Ovf_U4_Un;
		/// <summary>
		/// Converts the unsigned value on top of the evaluation stack to unsigned int64, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_U8_Un = Mono.Cecil.Cil.OpCodes.Conv_Ovf_U8_Un;
		/// <summary>
		/// Converts the unsigned value on top of the evaluation stack to signed natural int, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_I_Un = Mono.Cecil.Cil.OpCodes.Conv_Ovf_I_Un;
		/// <summary>
		/// Converts the unsigned value on top of the evaluation stack to unsigned natural int, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_U_Un = Mono.Cecil.Cil.OpCodes.Conv_Ovf_U_Un;
		/// <summary>
		/// Converts a value type to an object reference (type O). 
		/// </summary>
		public static OpCode Box = Mono.Cecil.Cil.OpCodes.Box;
		/// <summary>
		/// Pushes an object reference to a new zero-based, one-dimensional array whose elements are of a specific type onto the evaluation stack. 
		/// </summary>
		public static OpCode Newarr = Mono.Cecil.Cil.OpCodes.Newarr;
		/// <summary>
		/// Pushes the number of elements of a zero-based, one-dimensional array onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldlen = Mono.Cecil.Cil.OpCodes.Ldlen;
		/// <summary>
		/// Loads the element at a specified array index onto the top of the evaluation stack as the type specified in the instruction.
		/// </summary>
		//public static OpCode Ldelem = Mono.Cecil.Cil.OpCodes.Ldelem;
		/// <summary>
		/// Loads the address of the array element at a specified array index onto the top of the evaluation stack as type &amp; (managed pointer). 
		/// </summary>
		public static OpCode Ldelema = Mono.Cecil.Cil.OpCodes.Ldelema;
		/// <summary>
		/// Loads the element with type int8 at a specified array index onto the top of the evaluation stack as an int32. 
		/// </summary>
		public static OpCode Ldelem_I1 = Mono.Cecil.Cil.OpCodes.Ldelem_I1;
		/// <summary>
		/// Loads the element with type unsigned int8 at a specified array index onto the top of the evaluation stack as an int32.
		/// </summary>
		public static OpCode Ldelem_U1 = Mono.Cecil.Cil.OpCodes.Ldelem_U1;
		/// <summary>
		/// Loads the element with type int16 at a specified array index onto the top of the evaluation stack as an int32.
		/// </summary>
		public static OpCode Ldelem_I2 = Mono.Cecil.Cil.OpCodes.Ldelem_I2;
		/// <summary>
		/// Loads the element with type unsigned int16 at a specified array index onto the top of the evaluation stack as an int32. 
		/// </summary>
		public static OpCode Ldelem_U2 = Mono.Cecil.Cil.OpCodes.Ldelem_U2;
		/// <summary>
		/// Loads the element with type int32 at a specified array index onto the top of the evaluation stack as an int32. 
		/// </summary>
		public static OpCode Ldelem_I4 = Mono.Cecil.Cil.OpCodes.Ldelem_I4;
		/// <summary>
		/// Loads the element with type unsigned int32 at a specified array index onto the top of the evaluation stack as an int32. 
		/// </summary>
		public static OpCode Ldelem_U4 = Mono.Cecil.Cil.OpCodes.Ldelem_U4;
		/// <summary>
		/// Loads the element with type int64 at a specified array index onto the top of the evaluation stack as an int64.
		/// </summary>
		public static OpCode Ldelem_I8 = Mono.Cecil.Cil.OpCodes.Ldelem_I8;
		/// <summary>
		/// Loads the element with type natural int at a specified array index onto the top of the evaluation stack as a natural int. 
		/// </summary>
		public static OpCode Ldelem_I = Mono.Cecil.Cil.OpCodes.Ldelem_I;
		/// <summary>
		/// Loads the element with type float32 at a specified array index onto the top of the evaluation stack as type F (float). 
		/// </summary>
		public static OpCode Ldelem_R4 = Mono.Cecil.Cil.OpCodes.Ldelem_R4;
		/// <summary>
		/// Loads the element with type float64 at a specified array index onto the top of the evaluation stack as type F (float). 
		/// </summary>
		public static OpCode Ldelem_R8 = Mono.Cecil.Cil.OpCodes.Ldelem_R8;
		/// <summary>
		/// Loads the element containing an object reference at a specified array index onto the top of the evaluation stack as type O (object reference).
		/// </summary>
		public static OpCode Ldelem_Ref = Mono.Cecil.Cil.OpCodes.Ldelem_Ref;
		/// <summary>
		/// Replaces the array element at a given index with the natural int value on the evaluation stack. 
		/// </summary>
		public static OpCode Stelem_I = Mono.Cecil.Cil.OpCodes.Stelem_I;
		/// <summary>
		/// Replaces the array element at a given index with the int8 value on the evaluation stack. 
		/// </summary>
		public static OpCode Stelem_I1 = Mono.Cecil.Cil.OpCodes.Stelem_I1;
		/// <summary>
		/// Replaces the array element at a given index with the int16 value on the evaluation stack. 
		/// </summary>
		public static OpCode Stelem_I2 = Mono.Cecil.Cil.OpCodes.Stelem_I2;
		/// <summary>
		/// Replaces the array element at a given index with the int32 value on the evaluation stack. 
		/// </summary>
		public static OpCode Stelem_I4 = Mono.Cecil.Cil.OpCodes.Stelem_I4;
		/// <summary>
		/// Replaces the array element at a given index with the int64 value on the evaluation stack. 
		/// </summary>
		public static OpCode Stelem_I8 = Mono.Cecil.Cil.OpCodes.Stelem_I8;
		/// <summary>
		/// Replaces the array element at a given index with the float32 value on the evaluation stack. 
		/// </summary>
		public static OpCode Stelem_R4 = Mono.Cecil.Cil.OpCodes.Stelem_R4;
		/// <summary>
		/// Replaces the array element at a given index with the float64 value on the evaluation stack. 
		/// </summary>
		public static OpCode Stelem_R8 = Mono.Cecil.Cil.OpCodes.Stelem_R8;
		/// <summary>
		/// Replaces the array element at a given index with the object ref value (type O) on the evaluation stack. 
		/// </summary>
		public static OpCode Stelem_Ref = Mono.Cecil.Cil.OpCodes.Stelem_Ref;
		/// <summary>
		/// Converts the signed value on top of the evaluation stack to signed int8 and extends it to int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_I1 = Mono.Cecil.Cil.OpCodes.Conv_Ovf_I1;
		/// <summary>
		/// Converts the signed value on top of the evaluation stack to unsigned int8 and extends it to int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_U1 = Mono.Cecil.Cil.OpCodes.Conv_Ovf_U1;
		/// <summary>
		/// Converts the signed value on top of the evaluation stack to signed int16 and extending it to int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_I2 = Mono.Cecil.Cil.OpCodes.Conv_Ovf_I2;
		/// <summary>
		/// Converts the signed value on top of the evaluation stack to unsigned int16 and extends it to int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_U2 = Mono.Cecil.Cil.OpCodes.Conv_Ovf_U2;
		/// <summary>
		/// Converts the signed value on top of the evaluation stack to signed int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_I4 = Mono.Cecil.Cil.OpCodes.Conv_Ovf_I4;
		/// <summary>
		/// Converts the signed value on top of the evaluation stack to unsigned int32, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_U4 = Mono.Cecil.Cil.OpCodes.Conv_Ovf_U4;
		/// <summary>
		/// Converts the signed value on top of the evaluation stack to signed int64, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_I8 = Mono.Cecil.Cil.OpCodes.Conv_Ovf_I8;
		/// <summary>
		/// Converts the signed value on top of the evaluation stack to unsigned int64, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_U8 = Mono.Cecil.Cil.OpCodes.Conv_Ovf_U8;
		/// <summary>
		/// Retrieves the address (type &amp;) embedded in a typed reference.
		/// </summary>
		public static OpCode Refanyval = Mono.Cecil.Cil.OpCodes.Refanyval;
		/// <summary>
		/// Throws <see cref="System.ArithmeticException"/> if value is not a finite number. 
		/// </summary>
		public static OpCode Ckfinite = Mono.Cecil.Cil.OpCodes.Ckfinite;
		/// <summary>
		/// Pushes a typed reference to an instance of a specific type onto the evaluation stack. 
		/// </summary>
		public static OpCode Mkrefany = Mono.Cecil.Cil.OpCodes.Mkrefany;
		/// <summary>
		/// Converts a metadata token to its runtime representation, pushing it onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldtoken = Mono.Cecil.Cil.OpCodes.Ldtoken;
		/// <summary>
		/// Converts the value on top of the evaluation stack to unsigned int16, and extends it to int32. 
		/// </summary>
		public static OpCode Conv_U2 = Mono.Cecil.Cil.OpCodes.Conv_U2;
		/// <summary>
		/// Converts the value on top of the evaluation stack to unsigned int8, and extends it to int32. 
		/// </summary>
		public static OpCode Conv_U1 = Mono.Cecil.Cil.OpCodes.Conv_U1;
		/// <summary>
		/// Converts the value on top of the evaluation stack to natural int. 
		/// </summary>
		public static OpCode Conv_I = Mono.Cecil.Cil.OpCodes.Conv_I;
		/// <summary>
		/// Converts the signed value on top of the evaluation stack to signed natural int, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_I = Mono.Cecil.Cil.OpCodes.Conv_Ovf_I;
		/// <summary>
		/// Converts the signed value on top of the evaluation stack to unsigned natural int, throwing <see cref="System.OverflowException"/> on overflow. 
		/// </summary>
		public static OpCode Conv_Ovf_U = Mono.Cecil.Cil.OpCodes.Conv_Ovf_U;
		/// <summary>
		/// Adds two integers, performs an overflow check, and pushes the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Add_Ovf = Mono.Cecil.Cil.OpCodes.Add_Ovf;
		/// <summary>
		/// Adds two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Add_Ovf_Un = Mono.Cecil.Cil.OpCodes.Add_Ovf_Un;
		/// <summary>
		/// Multiplies two integer values, performs an overflow check, and pushes the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Mul_Ovf = Mono.Cecil.Cil.OpCodes.Mul_Ovf;
		/// <summary>
		/// Multiplies two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Mul_Ovf_Un = Mono.Cecil.Cil.OpCodes.Mul_Ovf_Un;
		/// <summary>
		/// Subtracts one integer value from another, performs an overflow check, and pushes the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Sub_Ovf = Mono.Cecil.Cil.OpCodes.Sub_Ovf;
		/// <summary>
		/// Subtracts one unsigned integer value from another, performs an overflow check, and pushes the result onto the evaluation stack. 
		/// </summary>
		public static OpCode Sub_Ovf_Un = Mono.Cecil.Cil.OpCodes.Sub_Ovf_Un;
		/// <summary>
		/// Transfers control from the fault or finally clause of an exception block back to the Common Language Infrastructure (CLI) exception handler. 
		/// </summary>
		public static OpCode Endfinally = Mono.Cecil.Cil.OpCodes.Endfinally;
		/// <summary>
		/// Exits a protected region of code, unconditionally transferring control to a specific target instruction. 
		/// </summary>
		public static OpCode Leave = Mono.Cecil.Cil.OpCodes.Leave;
		/// <summary>
		/// Exits a protected region of code, unconditionally transferring control to a target instruction (short form). 
		/// </summary>
		public static OpCode Leave_S = Mono.Cecil.Cil.OpCodes.Leave_S;
		/// <summary>
		/// Stores a value of type natural int at a supplied address. 
		/// </summary>
		public static OpCode Stind_I = Mono.Cecil.Cil.OpCodes.Stind_I;
		/// <summary>
		/// Converts the value on top of the evaluation stack to unsigned natural int, and extends it to natural int. 
		/// </summary>
		public static OpCode Conv_U = Mono.Cecil.Cil.OpCodes.Conv_U;
		/// <summary>
		/// Returns an unmanaged pointer to the argument list of the current method. 
		/// </summary>
		public static OpCode Arglist = Mono.Cecil.Cil.OpCodes.Arglist;
		/// <summary>
		/// Compares two values. If they are equal, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack. 
		/// </summary>
		public static OpCode Ceq = Mono.Cecil.Cil.OpCodes.Ceq;
		/// <summary>
		/// Compares two values. If the first value is greater than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack. 
		/// </summary>
		public static OpCode Cgt = Mono.Cecil.Cil.OpCodes.Cgt;
		/// <summary>
		/// Compares two unsigned or unordered values. If the first value is greater than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack. 
		/// </summary>
		public static OpCode Cgt_Un = Mono.Cecil.Cil.OpCodes.Cgt_Un;
		/// <summary>
		/// Compares two values. If the first value is less than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack. 
		/// </summary>
		public static OpCode Clt = Mono.Cecil.Cil.OpCodes.Clt;
		/// <summary>
		/// Compares the unsigned or unordered values value1 and value2. If value1 is less than value2, then the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack. 
		/// </summary>
		public static OpCode Clt_Un = Mono.Cecil.Cil.OpCodes.Clt_Un;
		/// <summary>
		/// Pushes an unmanaged pointer (type natural int) to the native code implementing a specific method onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldftn = Mono.Cecil.Cil.OpCodes.Ldftn;
		/// <summary>
		/// Pushes an unmanaged pointer (type natural int) to the native code implementing a particular virtual method associated with a specified object onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldvirtftn = Mono.Cecil.Cil.OpCodes.Ldvirtftn;
		/// <summary>
		/// Loads an argument (referenced by a specified index value) onto the stack. 
		/// </summary>
		public static OpCode Ldarg = Mono.Cecil.Cil.OpCodes.Ldarg;
		/// <summary>
		/// Load an argument address onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldarga = Mono.Cecil.Cil.OpCodes.Ldarga;
		/// <summary>
		/// Stores the value on top of the evaluation stack in the argument slot at a specified index. 
		/// </summary>
		public static OpCode Starg = Mono.Cecil.Cil.OpCodes.Starg;
		/// <summary>
		/// Loads the local variable at a specific index onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldloc = Mono.Cecil.Cil.OpCodes.Ldloc;
		/// <summary>
		/// Loads the address of the local variable at a specific index onto the evaluation stack. 
		/// </summary>
		public static OpCode Ldloca = Mono.Cecil.Cil.OpCodes.Ldloca;
		/// <summary>
		/// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at a specified index. 
		/// </summary>
		public static OpCode Stloc = Mono.Cecil.Cil.OpCodes.Stloc;
		/// <summary>
		/// Allocates a certain number of bytes from the local dynamic memory pool and pushes the address (a transient pointer, type *) of the first allocated byte onto the evaluation stack. 
		/// </summary>
		public static OpCode Localloc = Mono.Cecil.Cil.OpCodes.Localloc;
		/// <summary>
		/// Transfers control from the fault or finally clause of an exception block back to the Common Language Infrastructure (CLI) exception handler. 
		/// </summary>
		public static OpCode Endfilter = Mono.Cecil.Cil.OpCodes.Endfilter;
		/// <summary>
		/// Indicates that an address currently atop the evaluation stack might not be aligned to the natural size of the immediately following ldind, stind, ldfld, stfld, ldobj, stobj, initblk, or cpblk instruction. 
		/// </summary>
		public static OpCode Unaligned = Mono.Cecil.Cil.OpCodes.Unaligned;
		/// <summary>
		/// Specifies that an address currently atop the evaluation stack might be volatile, and the results of reading that location cannot be cached or that multiple stores to that location cannot be suppressed. 
		/// </summary>
		public static OpCode Volatile = Mono.Cecil.Cil.OpCodes.Volatile;
		/// <summary>
		/// Performs a postfixed method call instruction such that the current method's stack frame is removed before the actual call instruction is executed. 
		/// </summary>
		public static OpCode Tailcall = Mono.Cecil.Cil.OpCodes.Tail;
		/// <summary>
		/// Initializes all the fields of the object at a specific address to a null reference or a 0 of the appropriate primitive type. 
		/// </summary>
		public static OpCode Initobj = Mono.Cecil.Cil.OpCodes.Initobj;
		/// <summary>
		/// Constrains the type on which a virtual method call is made. 
		/// </summary>
		public static OpCode Constrained = Mono.Cecil.Cil.OpCodes.Constrained;
		/// <summary>
		/// Copies a specified number bytes from a source address to a destination address. 
		/// </summary>
		public static OpCode Cpblk = Mono.Cecil.Cil.OpCodes.Cpblk;
		/// <summary>
		/// Initializes a specified block of memory at a specific address to a given size and initial value.
		/// </summary>
		public static OpCode Initblk = Mono.Cecil.Cil.OpCodes.Initblk;
		/// <summary>
		/// Rethrows the current exception. 
		/// </summary>
		public static OpCode Rethrow = Mono.Cecil.Cil.OpCodes.Rethrow;
		/// <summary>
		/// Pushes the size, in bytes, of a supplied value type onto the evaluation stack. 
		/// </summary>
		public static OpCode Sizeof = Mono.Cecil.Cil.OpCodes.Sizeof;
		/// <summary
		/// >Retrieves the type token embedded in a typed reference. 
		/// </summary>
		public static OpCode Refanytype = Mono.Cecil.Cil.OpCodes.Refanytype;
		/// <summary>
		/// Specifies that the subsequent array address operation performs no type check at run time, and that it returns a managed pointer whose mutability is restricted. 
		/// </summary>
		public static OpCode Readonly = Mono.Cecil.Cil.OpCodes.Readonly;
	}
}
