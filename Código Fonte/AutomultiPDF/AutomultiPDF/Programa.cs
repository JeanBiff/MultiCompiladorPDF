using System;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;

class Program
{
    static bool usingForms;
    static bool userDefinableFolderName;
    static readonly InterfaceVisual.InterfaceVisual interfaceVisual = new InterfaceVisual.InterfaceVisual();

    [STAThread]
    static void Main(string[] args)
    {
        bool u_userDefinableFolderName = Convert.ToBoolean(ConfigurationManager.AppSettings["UserDefinableFolderName"]);
        bool u_usingForms = Convert.ToBoolean(ConfigurationManager.AppSettings["UsingForms"]);


        usingForms = u_usingForms;
        userDefinableFolderName = u_userDefinableFolderName;

        bool runAgain = true;

        if (!usingForms)
        {
            Console.Title = ("MultiCompiladorPDF");
            Console.ForegroundColor = ConsoleColor.Blue;

            while (runAgain)
            {
                Console.Clear();
                LogoPage();
                string outputFolderName = "0_MANUAL COMPLETO";
                if (userDefinableFolderName)
                {
                    Console.Write(">> Nome da pasta: ");
                    outputFolderName = Console.ReadLine();
                    Console.Clear();
                    LogoPage();
                }


                Console.Write(">> Pasta raíz: ");
                string rootFolder = Console.ReadLine();

                while (string.IsNullOrEmpty(rootFolder) || !Directory.Exists(rootFolder))
                {
                    Console.Clear();
                    LogoPage();
                    Console.Write(">> Inválido, digite novamete: ");
                    rootFolder = Console.ReadLine();
                }

                string outputFolder = Path.Combine(rootFolder, outputFolderName);
                Directory.CreateDirectory(outputFolder);

                ProcessFolders(rootFolder, outputFolder);

                Console.Clear();
                LogoPage();
                Console.Write(">> Projetos compilados com sucesso. Deseja compilar outro? (S/N): ");
                string answer = Console.ReadLine();
                if (answer == "n" || answer == "N")
                {
                    runAgain = false;
                }
            }
        }
        else
        {
            Console.WindowHeight = 5;
            Console.WindowWidth = 15;
            Application.Run(interfaceVisual);
        }
    }

    static void ProcessFolders(string rootFolder, string outputFolder)
    {
        string[] folders = Directory.GetDirectories(rootFolder);

        foreach (string folder in folders)
        {
            string[] pdfFiles = Directory.GetFiles(folder, "*.pdf");

            if (pdfFiles.Length > 0)
            {
                string outputFileName = Path.Combine(outputFolder, $"{Path.GetFileName(folder)}.pdf");
                MergePDFs(pdfFiles, outputFileName);
            }
        }
    }

    static void MergePDFs(string[] filesToMerge, string outputFilePath)
    {
        Loading();
        using (FileStream stream = new FileStream(outputFilePath, FileMode.Create))
        {
            Document document = new Document();
            PdfCopy pdf = new PdfCopy(document, stream);
            document.Open();

            foreach (string file in filesToMerge)
            {
                PdfReader reader = new PdfReader(file);
                pdf.AddDocument(reader);
                reader.Close();
            }

            pdf.Close();
            document.Close();
        }
    }
    
    static void LogoPage()
    {
        Console.WriteLine("MultiCompilador PDF ver 1.1");
        Console.WriteLine("                                                                                               %    ");
        Console.WriteLine("                                               ,,,,######,,,*                              (((((((( ");
        Console.WriteLine("             %%%%%%%%%%%%%###########################################################   %%%#########");
        Console.WriteLine("          %%%%%%%%%%%%%%%%%%%%%%%#################################################   %%%%%%%%%%%%%% ");
        Console.WriteLine("        %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%##########################################     %%%%%%       ");
        Console.WriteLine("       &&&%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%##################################(#############     ");
        Console.WriteLine("      &&&&&&               %%%%%%              *%%%%%*               ####/              #######     ");
        Console.WriteLine("     &&&&&&      &&&&&      %%%%%%%%%%%%%%      %%#      %%%%#      ###      #####      ######      ");
        Console.WriteLine("    &&&&&&      &&&&&&      &&&%%%             %%      %%%%%%      %#      #######      #####       ");
        Console.WriteLine("   &&&&&&      &&&&&&      &&      %&&%%      %%      %%%%%%      %%                   #####        ");
        Console.WriteLine("  &&&&&&      &&&&&&      &      &&&&&&      %%%      %%%%%      %%%      %%%%%%%%#########         ");
        Console.WriteLine(" &&&&&&      &&&#       &&&       &&&&      &&&&&               %%%%                %%%%##          ");
        Console.WriteLine(" &&&&&              &&&&&&&&&            *&&&&&&&&&&&&&&&      %%%%%%%%           %%%%%%%           ");
        Console.WriteLine(" &&&&      &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&               &&&&&%%%%%%%%%%%%%%%%%%%%%%            ");
        Console.WriteLine("   &      &&&&&#                                                       %&%%%%%%%%%%%%%%             ");
        Console.WriteLine();
    }

    static void Loading()
    {
        Console.WriteLine(">> Juntando PDFs...");
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write("");
        Console.SetCursorPosition(0, Console.CursorTop);
    }
}