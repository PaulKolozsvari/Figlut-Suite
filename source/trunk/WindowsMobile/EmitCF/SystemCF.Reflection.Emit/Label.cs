using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SystemCF.Reflection;

namespace SystemCF.Reflection.Emit
{
	/// <summary>
	/// Represents a label in the instruction stream. <see cref="Label"/> is used in conjunction with the <see cref="ILGenerator"/> class. 
	/// </summary>
	public class Label
	{
		#region Private&Internal
		Instruction instr;
		internal Instruction Instr
		{
			get
			{
				Label lbl = this;
				while (lbl != null)
				{
					Instruction instr = lbl.instr;
					if (instr != null) return instr;
					lbl = lbl.linked;
				}
				return null;
			}
			set
			{
				instr = value;
			}
		}

		internal Label linked;
		internal bool isMarked;

		internal bool IsValid()
		{
			return (instr != null);
		}

		internal static bool AreValid(Label[] labels)
		{
			foreach (Label label in labels)
				if (!label.IsValid())
					return false;
			return true;
		}

		internal static Instruction[] Instrs(Label[] labels)
		{
			List<Instruction> instrs = new List<Instruction>();
			foreach (Label label in labels)
				instrs.Add(label.instr);
			return instrs.ToArray();
		}
		#endregion
	}
}
