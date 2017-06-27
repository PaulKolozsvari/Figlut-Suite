namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    #endregion //Using Directives

    static public class Instantiator<E>
    {
        #region Constructors

        static Instantiator()
        {
            //TODO Read up on anonymous methods, inline Lambda expressions and expression trees: http://www.codeproject.com/Articles/17575/Lambda-Expressions-and-Expression-Trees-An-Introdu
            Type t = typeof(E);
            if (!(t.IsValueType || (t.IsClass && !t.IsAbstract)))
            {
                throw new ArgumentException(string.Concat("The type ", typeof(E).Name, " is not constructable."));
            }
        }

        #endregion //Constructors

        #region Methods

        public static E New()
        {
            return Expression.Lambda<Func<E>>(Expression.New(typeof(E))).Compile()();
        }

        public static E New<TA>(TA valueA)
        {
            return Instantiator<E>.CreateLambdaExpression<Func<TA, E>>(typeof(TA)).Compile()(valueA);
        }

        public static E New<TA, TB>(TA valueA, TB valueB)
        {
            return Instantiator<E>.CreateLambdaExpression<Func<TA, TB, E>>(typeof(TA), typeof(TB)).Compile()(valueA, valueB);
        }

        public static E New<TA, TB, TC>(TA valueA, TB valueB, TC valueC)
        {
            return Instantiator<E>.CreateLambdaExpression<Func<TA, TB, TC, E>>(typeof(TA), typeof(TB), typeof(TC)).Compile()(valueA, valueB, valueC);
        }

        public static E New<TA, TB, TC, TD>(TA valueA, TB valueB, TC valueC, TD valueD)
        {
            return Instantiator<E>.CreateLambdaExpression<Func<TA, TB, TC, TD, E>>(typeof(TA), typeof(TB), typeof(TC), typeof(TD)).Compile()(valueA, valueB, valueC, valueD);
        }

        static private Expression<TDelegate> CreateLambdaExpression<TDelegate>(params Type[] argTypes)
        {
            Debug.Assert(argTypes != null);
            if (argTypes == null)
            {
                throw new NullReferenceException("No arguments supplied to create a the lambda expression.");
            }
            ParameterExpression[] paramExpressions = new ParameterExpression[argTypes.Length];
            for (int i = 0; i < paramExpressions.Length; i++)
            {
                paramExpressions[i] = Expression.Parameter(argTypes[i], string.Concat("arg", i));
            }
            ConstructorInfo ctorInfo = typeof(E).GetConstructor(argTypes);
            if (ctorInfo == null)
            {
                throw new ArgumentException(String.Concat("The type ", typeof(E).Name, " has no constructor with the argument type(s) ", String.Join(", ", argTypes.Select(t => t.Name).ToArray()), "."),
                        "argTypes");
            }
            return Expression.Lambda<TDelegate>(Expression.New(ctorInfo, paramExpressions), paramExpressions);
        }

        #endregion //Methods
    }
}