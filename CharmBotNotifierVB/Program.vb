Imports System
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms

Public Module Program
    <STAThread>
    Public Sub Main()
        AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf ResolveAssembly

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New Form1())
    End Sub

    Private Function ResolveAssembly(sender As Object, args As ResolveEventArgs) As Assembly
        Dim requestedName As New AssemblyName(args.Name)
        Dim dllName = requestedName.Name & ".dll"

        ' 1. Tentar ler de subpastas organizadas (data, dados, recursos, assets, lib, bin)
        Dim baseDir = AppDomain.CurrentDomain.BaseDirectory
        Dim searchDirs = {"data", "dados", "recursos", "assets", "lib", "bin"}
        For Each d In searchDirs
            Dim p = Path.Combine(baseDir, d, dllName)
            If File.Exists(p) Then
                Try
                    Return Assembly.LoadFrom(p)
                Catch
                End Try
            End If
        Next

        ' 2. Tentar carregar de recurso embutido no próprio .exe
        Dim thisAsm = Assembly.GetExecutingAssembly()
        For Each res In thisAsm.GetManifestResourceNames()
            If res.EndsWith("." & dllName, StringComparison.OrdinalIgnoreCase) OrElse res.Equals(dllName, StringComparison.OrdinalIgnoreCase) Then
                Using stream = thisAsm.GetManifestResourceStream(res)
                    If stream IsNot Nothing Then
                        Dim buffer(CInt(stream.Length - 1)) As Byte
                        stream.Read(buffer, 0, buffer.Length)
                        Return Assembly.Load(buffer)
                    End If
                End Using
            End If
        Next

        Return Nothing
    End Function
End Module
