using System;
using System.Reflection;
using System.Windows.Forms;

namespace GUtils.Forms
{
    // Credit goes to http://www.lyquidity.com/devblog/?p=136
    /// <summary>
    /// Wraps System.Windows.Forms.OpenFileDialog to make it
    /// present a vista-style dialog.
    /// </summary>
    public class FolderSelectDialog
    {
        // Wrapped dialog
        private readonly OpenFileDialog ofd;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FolderSelectDialog ( )
        {
            ofd = new OpenFileDialog
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
        public string InitialDirectory
        {
            get { return ofd.InitialDirectory; }
            set { ofd.InitialDirectory = value == null || value.Length == 0 ? Environment.CurrentDirectory : value; }
        }

        /// <summary>
        /// Gets/Sets the title to show in the dialog
        /// </summary>
        public string Title
        {
            get { return ofd.Title; }
            set { ofd.Title = value ?? "Select a folder"; }
        }

        /// <summary>
        /// Gets the selected folder
        /// </summary>
        public string FileName
        {
            get { return ofd.FileName; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Shows the dialog
        /// </summary>
        /// <returns>True if the user presses OK else false</returns>
        public bool ShowDialog ( )
        {
            return ShowDialog ( IntPtr.Zero );
        }

        /// <summary>
        /// Shows the dialog
        /// </summary>
        /// <param name="hWndOwner">
        /// Handle of the control to be parent
        /// </param>
        /// <returns>True if the user presses OK else false</returns>
        public bool ShowDialog ( IntPtr hWndOwner )
        {
            var flag = false;

            if ( Environment.OSVersion.Version.Major >= 6 )
            {
                var r = new Reflector ( "System.Windows.Forms" );

                var num = 0U;
                var typeIFileDialog = r.GetType ( "FileDialogNative.IFileDialog" );
                var dialog = Reflector.Call ( ofd, "CreateVistaDialog" );
                Reflector.Call ( ofd, "OnBeforeVistaDialog", dialog );

                var options = ( uint ) Reflector.CallAs ( typeof ( FileDialog ), ofd, "GetOptions" );
                options |= ( uint ) r.GetEnum ( "FileDialogNative.FOS", "FOS_PICKFOLDERS" );
                Reflector.CallAs ( typeIFileDialog, dialog, "SetOptions", options );

                var pfde = r.New ( "FileDialog.VistaDialogEvents", ofd );
                var parameters = new object[] { pfde, num };
                Reflector.CallAs2 ( typeIFileDialog, dialog, "Advise", parameters );
                num = ( uint ) parameters[1];
                try
                {
                    var num2 = ( int ) Reflector.CallAs ( typeIFileDialog, dialog, "Show", hWndOwner );
                    flag = 0 == num2;
                }
                finally
                {
                    Reflector.CallAs ( typeIFileDialog, dialog, "Unadvise", num );
                    GC.KeepAlive ( pfde );
                }
            }
            else
            {
                using ( var fbd = new FolderBrowserDialog
                {
                    Description = Title,
                    SelectedPath = InitialDirectory,
                    ShowNewFolderButton = false
                } )
                {
                    if ( fbd.ShowDialog ( new WindowWrapper ( hWndOwner ) ) != DialogResult.OK )
                        return false;
                    ofd.FileName = fbd.SelectedPath;
                    flag = true;
                }
            }

            return flag;
        }

        #endregion Methods
    }

    /// <summary>
    /// Creates IWin32Window around an IntPtr
    /// </summary>
    public class WindowWrapper : IWin32Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="handle">Handle to wrap</param>
        public WindowWrapper ( IntPtr handle )
        {
            _hwnd = handle;
        }

        /// <summary>
        /// Original ptr
        /// </summary>
        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private readonly IntPtr _hwnd;
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
        public Reflector ( string ns )
            : this ( ns, ns )
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="an">
        /// A specific assembly name (used if the assembly name
        /// does not tie exactly with the namespace)
        /// </param>
        /// <param name="ns">
        /// The namespace containing types to be used
        /// </param>
        public Reflector ( String an, String ns )
        {
            m_ns = ns;
            m_asmb = null;
            foreach ( AssemblyName aN in Assembly.GetExecutingAssembly ( ).GetReferencedAssemblies ( ) )
            {
                if ( aN.FullName.StartsWith ( an ) )
                {
                    m_asmb = Assembly.Load ( aN );
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
            var names = typeName.Split ( '.' );

            if ( names.Length > 0 )
                type = m_asmb.GetType ( m_ns + "." + names[0] );

            for ( int i = 1 ; i < names.Length ; ++i )
            {
                type = type.GetNestedType ( names[i], BindingFlags.NonPublic );
            }
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
            var type = GetType ( name );

            var ctorInfos = type.GetConstructors ( );
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
            return Call2 ( obj, func, parameters );
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
        public static Object Call2 ( Object obj, String func, Object[] parameters )
        {
            return CallAs2 ( obj.GetType ( ), obj, func, parameters );
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
            return CallAs2 ( type, obj, func, parameters );
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
        public static Object CallAs2 ( Type type, Object obj, String func, Object[] parameters )
        {
            var methInfo = type.GetMethod ( func, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
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
            var propInfo = type.GetProperty ( prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
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
            var type = GetType ( typeName );
            var fieldInfo = type.GetField ( name );
            return fieldInfo.GetValue ( null );
        }

        #endregion Methods
    }
}