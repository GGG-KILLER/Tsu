using System;
using System.Linq;
using System.Reflection;

namespace GUtils.CLI.Commands
{
    public class Command
    {
        private readonly static Type TypeS = typeof ( String );
        public readonly String Name;
        public readonly MethodInfo Method;
        private readonly Object Instance;

        public Command ( String Name, MethodInfo Method, Object instance = null )
        {
            if ( String.IsNullOrWhiteSpace ( Name ) )
                throw new ArgumentException ( "Command name cannot be empty.", nameof ( Name ) );

            this.Name = Name;
            this.Method = Method ?? throw new ArgumentNullException ( nameof ( Method ) );
            this.Instance = instance;

            ParameterInfo[] @params = this.Method.GetParameters ( );
            for ( var i = 0; i < @params.Length; i++ )
                if ( Attribute.IsDefined ( @params[i], typeof ( CommandArgumentRestAttribute ) ) && i != @params.Length - 1 )
                    throw new Exception ( "[CommandArgumentRest] Should only be used on the last parameter of a Method." );
        }

        private static Object ChangeType ( String value, Type type )
        {
            return type == TypeS
                ? value
                : type.IsEnum
                    ? Enum.Parse ( type, value[0].ToString ( ).ToUpper ( ) + value.Substring ( 1 ) )
                    : Convert.ChangeType ( value, type );
        }

        public Object Invoke ( String[] arguments )
        {
            ParameterInfo[] methodParams = this.Method.GetParameters ( );
            Object[] args = new Object[methodParams.Length];

            for ( var i = 0; i < methodParams.Length; i++ )
            {
                if ( i > arguments.Length - 1 )
                {
                    if ( methodParams[i].HasDefaultValue )
                    {
                        args[i] = methodParams[i].DefaultValue;
                    }
                    else
                    {
                        throw new Exception ( $"Missing argument for {methodParams[i].Name}." );
                    }
                }
                else if ( Attribute.IsDefined ( this.Method, typeof ( CommandArgumentRestAttribute ) ) )
                {
                    var rest = String.Join ( " ", arguments.Skip ( i - 1 ) );
                    try
                    {
                        args[i] = ChangeType ( rest, methodParams[i].ParameterType );
                        break;
                    }
                    catch ( Exception e )
                    {
                        throw new Exception ( $"Error while attempting to change the type \"{rest}\" to {methodParams[i].ParameterType.Name}.", e );
                    }
                }
                else
                {
                    try
                    {
                        args[i] = ChangeType ( arguments[i], methodParams[i].ParameterType );
                    }
                    catch ( Exception e )
                    {
                        throw new Exception ( $"Error while attempting to change the type \"{arguments[i]}\" to {methodParams[i].ParameterType.Name}.", e );
                    }
                }
            }

            return this.Method.Invoke ( this.Instance, args );
        }
    }
}
