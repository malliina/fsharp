#light
namespace com.mle
open System
open System.Windows
open System.Windows.Forms
open System.IO
open System.Net
open System.Drawing

module MyWindow = 
    [<STAThread>]
    [<EntryPoint>]
    do
        let resource (r:WebResponse) (code: WebResponse -> 'T) =
            try
                code(r)
            finally
                r.Close()
        let http (url:String) = 
            let req = System.Net.WebRequest.Create(url)
            resource (req.GetResponse()) (fun (r:WebResponse) -> 
                let stream = r.GetResponseStream()
                let reader = new StreamReader(stream)
                reader.ReadToEnd()
            )
        let defaultSite = "http://www.live.com"
        let form = new Form(Visible=true,TopMost=true,Text="Welcome to F#!!!")
        let textField = new Windows.Forms.TextBox(Dock=DockStyle.Left)
        textField.Text <- defaultSite
        let button = new Button(Dock=DockStyle.Right)
        let textBox = new RichTextBox(Dock=DockStyle.Bottom,Text="Initial text")
        button.Click.Add(fun _ -> textBox.Text <- http textField.Text)
        button.Text <- "Go!"
        form.Controls.AddRange [| textField; button; textBox;  |]
        let google = http defaultSite
        textBox.Text <- google
        Application.Run(form)