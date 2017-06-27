using System.Collections.Generic;
using Mono.Cecil;
using SystemCF.Reflection;
using SystemCF.Reflection.Emit;

namespace SystemCF.Reflection
{
	internal class TypeAttributesConverter
	{
		static bool Match(Mono.Cecil.TypeAttributes value, Mono.Cecil.TypeAttributes @enum) { return (value & @enum) == @enum; }
		static bool Match(System.Reflection.TypeAttributes value, System.Reflection.TypeAttributes @enum) { return (value & @enum) == @enum; }

		public static System.Reflection.TypeAttributes Convert(Mono.Cecil.TypeAttributes peAttrs)
		{
			System.Reflection.TypeAttributes attrs = (System.Reflection.TypeAttributes) 0;
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.Abstract)) attrs |= System.Reflection.TypeAttributes.Abstract;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.AutoClass)) attrs |= System.Reflection.TypeAttributes.AutoClass;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.BeforeFieldInit)) attrs |= System.Reflection.TypeAttributes.BeforeFieldInit;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.ExplicitLayout)) attrs |= System.Reflection.TypeAttributes.ExplicitLayout;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.Import)) attrs |= System.Reflection.TypeAttributes.Import;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.Interface)) attrs |= System.Reflection.TypeAttributes.Interface;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.NestedAssembly)) attrs |= System.Reflection.TypeAttributes.NestedAssembly;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.NestedFamANDAssem)) attrs |= System.Reflection.TypeAttributes.NestedFamANDAssem;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.NestedFamily)) attrs |= System.Reflection.TypeAttributes.NestedFamily;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.NestedFamORAssem)) attrs |= System.Reflection.TypeAttributes.NestedFamORAssem;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.NestedPrivate)) attrs |= System.Reflection.TypeAttributes.NestedPrivate;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.NestedPublic)) attrs |= System.Reflection.TypeAttributes.NestedPublic;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.Public)) attrs |= System.Reflection.TypeAttributes.Public;			 
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.RTSpecialName)) attrs |= System.Reflection.TypeAttributes.RTSpecialName;	
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.Sealed)) attrs |= System.Reflection.TypeAttributes.Sealed;	
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.SequentialLayout)) attrs |= System.Reflection.TypeAttributes.SequentialLayout;	
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.Serializable)) attrs |= System.Reflection.TypeAttributes.Serializable;	
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.SpecialName)) attrs |= System.Reflection.TypeAttributes.SpecialName;	
			if (Match(peAttrs, Mono.Cecil.TypeAttributes.UnicodeClass)) attrs |= System.Reflection.TypeAttributes.UnicodeClass;	
			return attrs;
		}
		public static Mono.Cecil.TypeAttributes Convert(System.Reflection.TypeAttributes clrAttrs)
		{
			Mono.Cecil.TypeAttributes attrs = (Mono.Cecil.TypeAttributes) 0;
			if (Match(clrAttrs, System.Reflection.TypeAttributes.Abstract)) attrs |= Mono.Cecil.TypeAttributes.Abstract;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.AutoClass)) attrs |= Mono.Cecil.TypeAttributes.AutoClass;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.BeforeFieldInit)) attrs |= Mono.Cecil.TypeAttributes.BeforeFieldInit;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.ExplicitLayout)) attrs |= Mono.Cecil.TypeAttributes.ExplicitLayout;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.Import)) attrs |= Mono.Cecil.TypeAttributes.Import;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.Interface)) attrs |= Mono.Cecil.TypeAttributes.Interface;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.NestedAssembly)) attrs |= Mono.Cecil.TypeAttributes.NestedAssembly;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.NestedFamANDAssem)) attrs |= Mono.Cecil.TypeAttributes.NestedFamANDAssem;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.NestedFamily)) attrs |= Mono.Cecil.TypeAttributes.NestedFamily;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.NestedFamORAssem)) attrs |= Mono.Cecil.TypeAttributes.NestedFamORAssem;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.NestedPrivate)) attrs |= Mono.Cecil.TypeAttributes.NestedPrivate;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.NestedPublic)) attrs |= Mono.Cecil.TypeAttributes.NestedPublic;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.Public)) attrs |= Mono.Cecil.TypeAttributes.Public;			 
			if (Match(clrAttrs, System.Reflection.TypeAttributes.RTSpecialName)) attrs |= Mono.Cecil.TypeAttributes.RTSpecialName;	
			if (Match(clrAttrs, System.Reflection.TypeAttributes.Sealed)) attrs |= Mono.Cecil.TypeAttributes.Sealed;	
			if (Match(clrAttrs, System.Reflection.TypeAttributes.SequentialLayout)) attrs |= Mono.Cecil.TypeAttributes.SequentialLayout;	
			if (Match(clrAttrs, System.Reflection.TypeAttributes.Serializable)) attrs |= Mono.Cecil.TypeAttributes.Serializable;	
			if (Match(clrAttrs, System.Reflection.TypeAttributes.SpecialName)) attrs |= Mono.Cecil.TypeAttributes.SpecialName;	
			if (Match(clrAttrs, System.Reflection.TypeAttributes.UnicodeClass)) attrs |= Mono.Cecil.TypeAttributes.UnicodeClass;	
			return attrs;
		}
	}
}
