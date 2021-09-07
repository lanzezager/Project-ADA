/*
 * Creado por SharpDevelop.
 * Usuario: LZ-Job
 * Fecha: 14/08/2018
 * Hora: 03:43 p. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using System.Threading;
using ClosedXML;
using DocumentFormat.OpenXml;

namespace Nova_Gear
{
	/// <summary>
	/// Description of lector_pdf.
	/// </summary>
	public partial class lector_pdf : Form
	{
		public lector_pdf()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private Thread hilo2 = null;
		private Thread hilo3 = null;
		private Thread hilo4 = null;
		private Thread hilo5 = null;
		private Thread hilo6 = null;
		private Thread hilo7 = null;
		private Thread hilo8 = null;
		private Thread hilo9 = null;
		
		void Lector_pdfLoad(object sender, EventArgs e)
		{
			
		}
		
		String archivo,resultado="",buscar,Filename="";
		int inicio=0,fin=0,pri_cto=0,mitad=0,ter_cto=0,octa=0;
		
		public void Button4Click(object sender, EventArgs e)
		{
			if(textBox2.Text.Length==9){
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.Filter = "Archivos de PDF (*.pdf)|*.pdf"; //le indicamos el tipo de filtro en este caso que busque
				//solo los archivos excel
				dialog.Title = "Seleccione el archivo de PDF";//le damos un titulo a la ventana
				dialog.FileName = string.Empty;//inicializamos con vacio el nombre del archivo
				
				//si al seleccionar el archivo damos Ok
				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					archivo = dialog.FileName;
					//textBox1.Text=ReadPdfFile(archivo,textBox2.Text);
					gestor_hilos(archivo,textBox2.Text);
				}
			}
		}
		
		/*public string ReadPdfFile(object Filename)
		{
			//PdfReader reader2 = new PdfReader((string)Filename);
			string strText = string.Empty;
			PdfReader reader = new PdfReader((string)Filename);
			int numPaginas=reader.NumberOfPages;
			for (int page = 0; page <= numPaginas; page++)
			{
				ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
				String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
				s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
				strText = strText + s;
			}
			reader.Close();
			return strText;
		}*/
		
		public void gestor_hilos(string Filenamec,string buscarc){
			PdfReader reader = new PdfReader((string)Filenamec);
            int num_pages=reader.NumberOfPages;
            pri_cto=num_pages/4;
            octa=num_pages/8;
            mitad=pri_cto+pri_cto;
            ter_cto=pri_cto+pri_cto+pri_cto;
            fin=num_pages;
            Filename=Filenamec;
            buscar=buscarc;
            
            hilo2 = new Thread(new ThreadStart(ReadPdfFile));
			hilo2.Start();
			hilo3 = new Thread(new ThreadStart(ReadPdfFile1));
			hilo3.Start();
			hilo4 = new Thread(new ThreadStart(ReadPdfFile2));
			hilo4.Start();
			hilo5 = new Thread(new ThreadStart(ReadPdfFile3));
			hilo5.Start();
			hilo6 = new Thread(new ThreadStart(ReadPdfFile4));
			hilo6.Start();
			hilo7 = new Thread(new ThreadStart(ReadPdfFile5));
			hilo7.Start();
			hilo8 = new Thread(new ThreadStart(ReadPdfFile6));
			hilo8.Start();
			hilo9 = new Thread(new ThreadStart(ReadPdfFile7));
			hilo9.Start();
		}
		//1-1/8
		public void ReadPdfFile()
        {
            //PdfReader reader2 = new PdfReader((string)Filename);
            PdfReader reader = new PdfReader((string)Filename);
            int num_pages=reader.NumberOfPages;
            
            string strText = string.Empty;
            for (int page = 1; page <= octa; page++)
            {
                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                
                String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                Invoke(new MethodInvoker(delegate
					             {
                label2.Text="Busqueda 1: "+page;
                label2.Refresh();
                                         }));
                if(s.Length>200){
	                	if(s.Substring(s.Length-200,200).Contains(buscar)==true){
	                	strText="PAGINA "+(page+1);
	                	page=num_pages;
                	}
                }
                
                if(resultado.Length>0){
                	Invoke(new MethodInvoker(delegate
					             {
				                    textBox1.Text=resultado;     	
				                         }));
                	Thread.CurrentThread.Suspend();
                }
                //strText = strText + (s+"\r\nPAGINA "+(page+1)+" \r\n");
            }
            reader.Close();
            //return strText;
            resultado=strText;
            
        }
		//1/8-2/8
		public void ReadPdfFile1()
        {
            //PdfReader reader2 = new PdfReader((string)Filename);
            PdfReader reader = new PdfReader((string)Filename);
            int num_pages=reader.NumberOfPages;
            
            string strText = string.Empty;
            for (int page = octa; page <= pri_cto; page++)
            {
                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                
                String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                Invoke(new MethodInvoker(delegate
					             {
                label3.Text="Busqueda 2: "+page;
                label3.Refresh();}));
                
                if(s.Length>200){
	                	if(s.Substring(s.Length-200,200).Contains(buscar)==true){
	                	strText="PAGINA "+(page+1);
	                	page=num_pages;
                	}
                }
                
                if(resultado.Length>0){
                	Invoke(new MethodInvoker(delegate
					             {
				                    textBox1.Text=resultado;     	
				                         }));
                	Thread.CurrentThread.Suspend();
                }
                
                //strText = strText + (s+"\r\nPAGINA "+(page+1)+" \r\n");
            }
            reader.Close();
            //return strText;
            resultado=strText;
        }
		//2/8-3/8
		public void ReadPdfFile2()
        {
            //PdfReader reader2 = new PdfReader((string)Filename);
            PdfReader reader = new PdfReader((string)Filename);
            int num_pages=reader.NumberOfPages;
            
            string strText = string.Empty;
            for (int page = pri_cto; page <= (pri_cto+octa); page++)
            {
                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                
                String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                Invoke(new MethodInvoker(delegate
					             {
                label4.Text="Busqueda 3: "+page;
                label4.Refresh();
                                         }));
                if(s.Length>200){
	                	if(s.Substring(s.Length-200,200).Contains(buscar)==true){
	                	strText="PAGINA "+(page+1);
	                	page=num_pages;
                	}
                }
                
                if(resultado.Length>0){
                	Invoke(new MethodInvoker(delegate
					             {
				                    textBox1.Text=resultado;     	
				                         }));
                	Thread.CurrentThread.Suspend();
                }
                //strText = strText + (s+"\r\nPAGINA "+(page+1)+" \r\n");
            }
            reader.Close();
           //return strText;
            resultado=strText;
        }
		//3/8-4/8
		public void ReadPdfFile3()
        {
            //PdfReader reader2 = new PdfReader((string)Filename);
            PdfReader reader = new PdfReader((string)Filename);
            int num_pages=reader.NumberOfPages;
            
            string strText = string.Empty;
            for (int page = (pri_cto+octa); page <= mitad; page++)
            {
                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                
                String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                Invoke(new MethodInvoker(delegate
					             {
                label5.Text="Busqueda 4: "+page;
                label5.Refresh();
                                         }));
                if(s.Length>200){
	                	if(s.Substring(s.Length-200,200).Contains(buscar)==true){
	                	strText="PAGINA "+(page+1);
	                	page=num_pages;
                	}
                }
                
                if(resultado.Length>0){
                	Invoke(new MethodInvoker(delegate
					             {
				                    textBox1.Text=resultado;     	
				                         }));
                	Thread.CurrentThread.Suspend();
                }
                //strText = strText + (s+"\r\nPAGINA "+(page+1)+" \r\n");
            }
            reader.Close();
           //return strText;
            resultado=strText;
        }
		//4/8-5/8
		public void ReadPdfFile4()
        {
            //PdfReader reader2 = new PdfReader((string)Filename);
            PdfReader reader = new PdfReader((string)Filename);
            int num_pages=reader.NumberOfPages;
            
            string strText = string.Empty;
            for (int page = mitad; page <= (mitad+octa); page++)
            {
                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                
                String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                Invoke(new MethodInvoker(delegate
					             {
                label6.Text="Busqueda 5: "+page;
                label6.Refresh();
                                         }));
                if(s.Length>200){
	                	if(s.Substring(s.Length-200,200).Contains(buscar)==true){
	                	strText="PAGINA "+(page+1);
	                	page=num_pages;
                	}
                }
                
                if(resultado.Length>0){
                	Invoke(new MethodInvoker(delegate
					             {
				                    textBox1.Text=resultado;     	
				                         }));
                	Thread.CurrentThread.Suspend();
                }
                //strText = strText + (s+"\r\nPAGINA "+(page+1)+" \r\n");
            }
            reader.Close();
           //return strText;
            resultado=strText;
        }
		//5/8-6/8
		public void ReadPdfFile5()
        {
            //PdfReader reader2 = new PdfReader((string)Filename);
            PdfReader reader = new PdfReader((string)Filename);
            int num_pages=reader.NumberOfPages;
            
            string strText = string.Empty;
            for (int page = (mitad+octa); page <= ter_cto; page++)
            {
                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                
                String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                Invoke(new MethodInvoker(delegate
					             {
                label7.Text="Busqueda 6: "+page;
                label7.Refresh();
                                         }));
                if(s.Length>200){
	                	if(s.Substring(s.Length-200,200).Contains(buscar)==true){
	                	strText="PAGINA "+(page+1);
	                	page=num_pages;
                	}
                }
                
                if(resultado.Length>0){
                	Invoke(new MethodInvoker(delegate
					             {
				                    textBox1.Text=resultado;     	
				                         }));
                	Thread.CurrentThread.Suspend();
                }
                //strText = strText + (s+"\r\nPAGINA "+(page+1)+" \r\n");
            }
            reader.Close();
           //return strText;
            resultado=strText;
        }
		//6/8-7/8
		public void ReadPdfFile6()
        {
            //PdfReader reader2 = new PdfReader((string)Filename);
            PdfReader reader = new PdfReader((string)Filename);
            int num_pages=reader.NumberOfPages;
            
            string strText = string.Empty;
            for (int page = ter_cto; page <= (ter_cto+octa); page++)
            {
                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                
                String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                Invoke(new MethodInvoker(delegate
					             {
                label8.Text="Busqueda 7: "+page;
                label8.Refresh();
                                         }));
                if(s.Length>200){
	                	if(s.Substring(s.Length-200,200).Contains(buscar)==true){
	                	strText="PAGINA "+(page+1);
	                	page=num_pages;
                	}
                }
                
                if(resultado.Length>0){
                	Invoke(new MethodInvoker(delegate
					             {
				                    textBox1.Text=resultado;     	
				                         }));
                	Thread.CurrentThread.Suspend();
                }
                //strText = strText + (s+"\r\nPAGINA "+(page+1)+" \r\n");
            }
            reader.Close();
           //return strText;
            resultado=strText;
        }
		//7/8-8/8
		public void ReadPdfFile7()
        {
            //PdfReader reader2 = new PdfReader((string)Filename);
            PdfReader reader = new PdfReader((string)Filename);
            int num_pages=reader.NumberOfPages;
            
            string strText = string.Empty;
            for (int page = (ter_cto+octa); page <= fin; page++)
            {
                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                
                String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                Invoke(new MethodInvoker(delegate
					             {
                label9.Text="Busqueda 8: "+page;
                label9.Refresh();
                                         }));
                if(s.Length>200){
	                	if(s.Substring(s.Length-200,200).Contains(buscar)==true){
	                	strText="PAGINA "+(page+1);
	                	page=num_pages;
                	}
                }
                
                if(resultado.Length>0){
                	Invoke(new MethodInvoker(delegate
					             {
				                    textBox1.Text=resultado;     	
				                         }));
                	Thread.CurrentThread.Suspend();
                }
                //strText = strText + (s+"\r\nPAGINA "+(page+1)+" \r\n");
            }
            reader.Close();
           //return strText;
            resultado=strText;
        }
		
		public void gestor_resultados(){
			if(resultado.Length>0){
				hilo2.Suspend();
				hilo3.Suspend();
				hilo4.Suspend();
				hilo5.Suspend();
				Invoke(new MethodInvoker(delegate
					             {
				                    textBox1.Text=resultado;     	
				                         }));
			}
		}

        private void button1_Click(object sender, EventArgs e)
        {

        }
	}
}
