using System;
using System.Reflection;
using System.Windows.Forms;

namespace GUtils.Windows.Forms
{
    // Credit goes to http://www.lyquidity.com/devblog/?p=136
    // modified by GGG KILLER
    /// <summary>
    /// Wraps System.Windows.Forms.OpenFileDialog to make it
    /// present a vista-style dialog.
    /// </summary>
    public class FolderSelectDialog : IDisposable
    {
        private static readonly Reflector reflector;
        private static readonly Type IFileDialog;
        private static readonly UInt32 FOS_PICKFOLDERS;

        static FolderSelectDialog ( )
        {
            reflector = new Reflector ( "System.Windows.Forms" );
            IFileDialog = reflector.GetType ( "FileDialogNative.IFileDialog" );
            FOS_PICKFOLDERS = ( UInt32 ) reflector.GetEnum ( "FileDialogNative.FOS", "FOS_PICKFOLDERS" );
        }

        // Wrapped dialog
        private OpenFileDialog ofd;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FolderSelectDialog ( )
        {
            this.ofd = new OpenFileDialog
            {
                Filter = "Folders|\n",
                AddExtension = false,
                CheckFileExists = false,
                DereferenceLinks = true,
                Multiselect = false
            };
        }

        #region Properties

        /// <summary>
        /// Gets/Sets the initial folder to be selected. A null
        /// value selects the current directory.
        /// </summary>
        public String InitialDirectory
        {
            get { return this.ofd.InitialDirectory; }
            set { this.ofd.InitialDirectory = String.IsNullOrEmpty ( value ) ? Environment.CurrentDirectory : value; }
        }

        /// <summary>
        /// Gets/Sets the title to show in the dialog
        /// </summary>
        public String Title
        {
            get { return this.ofd.Title; }
            set { this.ofd.Title = value ?? "Select a folder"; }
        }

        /// <summary>
        /// Gets the selected folder
        /// </summary>
        public String FileName
        {
            get { return this.ofd.FileName; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Shows the dialog
        /// </summary>
        /// <param name="owner">Windows to parent the dialog</param>
        /// <returns>True if the user presses OK else false</returns>
        public Boolean ShowDialog ( IWin32Window owner = null )
        {
            if ( Environment.OSVersion.Version.Major >= 6 )
            {
                Object dialog = Reflector.Call ( this.ofd, "CreateVistaDialog" );
                Reflector.Call ( this.ofd, "OnBeforeVistaDialog", dialog );

                var options = ( UInt32 ) Reflector.CallAs ( typeof ( FileDialog ), this.ofd, "GetOptions" ) | FOS_PICKFOLDERS;
                Reflector.CallAs ( IFileDialog, dialog, "SetOptions", options );

                Object dialogEvents = reflector.New ( "FileDialog.VistaDialogEvents", this.ofd );
                var num = 0U;
                var parameters = new Object[] { dialogEvents, num };
                Reflector.CallAs ( IFileDialog, dialog, "Advise", parameters );
                num = ( UInt32 ) parameters[1];

                try
                {
                    return 0 == ( Int32 ) Reflector.CallAs ( IFileDialog, dialog, "Show", owner != null ? owner.Handle : IntPtr.Zero );
                }
                finally
                {
                    Reflector.CallAs ( IFileDialog, dialog, "Unadvise", num );
                    GC.KeepAlive ( dialogEvents );
                }
            }
            else
            {
                using ( var fbd = new FolderBrowserDialog
                {
                    Description = this.Title,
                    SelectedPath = this.InitialDirectory,
                    ShowNewFolderButton = false
                } )
                {
                    if ( ( owner != null ? fbd.ShowDialog ( owner ) : fbd.ShowDialog ( ) ) != DialogResult.OK )
                        return false;

                    this.ofd.FileName = fbd.SelectedPath;
                    return true;
                }
            }
        }

        #region IDisposable Support
        private Boolean disposedValue; // To detect redundant calls

        protected virtual void Dispose ( Boolean disposing )
        {
            if ( !this.disposedValue )
            {
                if ( disposing )
                {
                    this.ofd.Dispose ( );
                }

                this.ofd = null;
                this.disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool
        // disposing) above has code to free unmanaged resources.
        ~FolderSelectDialog ( )
        {
            // Do not change this code. Put cleanup code in
            // Dispose ( bool disposing ) above.
            Dispose ( false );
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose ( )
        {
            Dispose ( true );
            GC.SuppressFinalize ( this );
        }

        #endregion IDisposable Support

        #endregion Methods
    }

    /// <summary>
    /// This class is from the Front-End for Dosbox and is used to
    /// present a 'vista' dialog box to select folders. Being able
    /// to use a vista style dialog box to select folders is much
    /// better then using the shell folder browser. http://code.google.com/p/fed/
    ///
    /// Example: var r = new Reflector("System.Windows.Forms");
    /// </summary>
    public class Reflector
    {
        #region variables

        private readonly String m_ns;
        private readonly Assembly m_asmb;

        #endregion variables

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ns">
        /// The namespace containing types to be used
        /// </param>
        public Reflector ( String ns )
            : this ( ns, ns )
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assemblyName">
        /// A specific assembly name (used if the assembly name
        /// does not tie exactly with the namespace)
        /// </param>
        /// <param name="namespace">
        /// The namespace containing types to be used
        /// </param>
        public Reflector ( String assemblyName, String @namespace )
        {
            this.m_ns = @namespace;
            this.m_asmb = null;
            foreach ( AssemblyName aN in Assembly
                .GetExecutingAssembly ( )
                .GetReferencedAssemblies ( ) )
            {
                if ( aN.FullName.StartsWith ( assemblyName ) )
                {
                    this.m_asmb = Assembly.Load ( aN );
                    break;
                }
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Return a Type instance for a type 'typeName'
        /// </summary>
        /// <param name="typeName">The name of the type</param>
        /// <returns>A type instance</returns>
        public Type GetType ( String typeName )
        {
            Type type = null;
            String[] names = typeName.Split ( '.' );

            if ( names.Length > 0 )
                type = this.m_asmb.GetType ( $"{this.m_ns}.{names[0]}" );

            for ( var i = 1; i < names.Length; ++i )
                type = type.GetNestedType ( names[i], BindingFlags.NonPublic );

            return type;
        }

        /// <summary>
        /// Create a new object of a named type passing along any params
        /// </summary>
        /// <param name="name">The name of the type to create</param>
        /// <param name="parameters"></param>
        /// <returns>An instantiated type</returns>
        public Object New ( String name, params Object[] parameters )
        {
            ConstructorInfo[] ctorInfos = GetType ( name )
                .GetConstructors ( );
            foreach ( ConstructorInfo ci in ctorInfos )
            {
                try
                {
                    return ci.Invoke ( parameters );
                }
                catch ( Exception )
                {
                    continue;
                }
            }

            return null;
        }

        /// <summary>
        /// Calls method 'func' on object 'obj' passing parameters 'parameters'
        /// </summary>
        /// <param name="obj">
        /// The object on which to excute function 'func'
        /// </param>
        /// <param name="func">The function to execute</param>
        /// <param name="parameters">
        /// The parameters to pass to function 'func'
        /// </param>
        /// <returns>The result of the function invocation</returns>
        public static Object Call ( Object obj, String func, params Object[] parameters )
        {
            return CallAs ( obj.GetType ( ), obj, func, parameters );
        }

        /// <summary>
        /// Calls method 'func' on object 'obj' which is of type
        /// 'type' passing parameters 'parameters'
        /// </summary>
        /// <param name="type">The type of 'obj'</param>
        /// <param name="obj">
        /// The object on which to excute function 'func'
        /// </param>
        /// <param name="func">The function to execute</param>
        /// <param name="parameters">
        /// The parameters to pass to function 'func'
        /// </param>
        /// <returns>The result of the function invocation</returns>
        public static Object CallAs ( Type type, Object obj, String func, params Object[] parameters )
        {
            MethodInfo methInfo = type.GetMethod ( func, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
            return methInfo.Invoke ( obj, parameters );
        }

        /// <summary>
        /// Returns the value of property 'prop' of object 'obj'
        /// </summary>
        /// <param name="obj">The object containing 'prop'</param>
        /// <param name="prop">The property name</param>
        /// <returns>The property value</returns>
        public static Object Get ( Object obj, String prop )
        {
            return GetAs ( obj.GetType ( ), obj, prop );
        }

        /// <summary>
        /// Returns the value of property 'prop' of object 'obj'
        /// which has type 'type'
        /// </summary>
        /// <param name="type">The type of 'obj'</param>
        /// <param name="obj">The object containing 'prop'</param>
        /// <param name="prop">The property name</param>
        /// <returns>The property value</returns>
        public static Object GetAs ( Type type, Object obj, String prop )
        {
            PropertyInfo propInfo = type.GetProperty ( prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
            return propInfo.GetValue ( obj, null );
        }

        /// <summary>
        /// Returns an enum value
        /// </summary>
        /// <param name="typeName">The name of enum type</param>
        /// <param name="name">The name of the value</param>
        /// <returns>The enum value</returns>
        public Object GetEnum ( String typeName, String name )
        {
            Type type = GetType ( typeName );
            FieldInfo fieldInfo = type.GetField ( name );
            return fieldInfo.GetValue ( null );
        }

        #endregion Methods
    }
}
