/*
 * Creado por SharpDevelop.
 * Usuario: Lanze Zager
 * Fecha: 06/10/2015
 * Hora: 08:52 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Nova_Gear
{
	/// <summary>
	/// Description of About.
	/// </summary>
	public partial class About : Form
	{
		public About()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Panel2Paint(object sender, PaintEventArgs e)
		{
			
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			this.Dispose();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			//Registros.Foto_ampliada pic = new Registros.Foto_ampliada(global::Nova_Gear.Properties.Resources.jollyRoger,"© 2015-2016 Zager Developments.");
			Registros.Foto_ampliada pic = new Registros.Foto_ampliada("recursos/iconos/LZ_logo.png","© 2015-2016 Zager Developments.");
			pic.ShowDialog();
			
		}
		
		void Label2Click(object sender, EventArgs e)
		{
			
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			
		}
		
		public void actualizaciones(){
			
			String version, copyright,dont,lz,atlas,url,aux;
			int candado=0;
				
			try
			{
				StreamReader rdr = new StreamReader(@"NG_info.lz");
				version = @""+rdr.ReadLine();
				url = rdr.ReadLine();
                aux = rdr.ReadLine();
                aux = rdr.ReadLine();
                aux = rdr.ReadLine();
				copyright = rdr.ReadLine();
				dont = rdr.ReadLine();
				lz = rdr.ReadLine();
				atlas = rdr.ReadLine();
				rdr.Close();
				
				if(atlas.Equals("Arriba el Atlas!!!!!")){
					candado++;
				}
				if(lz.Equals("By LZ")){
					candado++;
				}
				if(dont.Equals("DON'T CHANGE THIS SETTINGS!!!!!")){
					candado++;
				}
				if(copyright.Equals("Nova Gear by Mario Espinoza & Miguel Bañuelos")){
					candado++;
				}
				
				if(candado==4){
					version=version.Substring(8,version.Length-8);
					label2.Text="Version: "+version;
				}else{
					label2.Text="Version: DESCONOCIDA";
						//MessageBox.Show("No se puede verificar la versión del programa.","AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
				}
				
			}catch(Exception error){
				MessageBox.Show("Ocurrió un Error al momento de verificar la version del programa:\n\n"+error,"AVISO",MessageBoxButtons.OK,MessageBoxIcon.Error);
				
			}
		}
		
		void AboutLoad(object sender, EventArgs e)
		{
			actualizaciones();
		}
		
		void PictureBox2Click(object sender, EventArgs e)
		{
			//Generador_factura gen = new Generador_factura();
			//gen.Show();
			Mod40.Carga_excel40 carga40 = new Mod40.Carga_excel40();
			carga40.Show();
			
			//Lector_rale_txt ralext= new Lector_rale_txt(1);
			//ralext.Show();
			//Estrados.Menu_estrados menu_e = new Nova_Gear.Estrados.Menu_estrados();
			//menu_e.Show();
			//Depuracion.Depuración2 depu2 = new Nova_Gear.Depuracion.Depuración2();
			//depu2.Show();
			
		}
	}
}
