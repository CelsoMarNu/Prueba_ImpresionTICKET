using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prueba_ImpresionTICKET
{
    public partial class Form1 : Form
    {
        DataTable detalles = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }
        public void ArmarDataTable()
        {
            DataColumn ColLinea;
            ColLinea = new DataColumn("LINEA");

            DataColumn ColCodigo;
            ColCodigo = new DataColumn("CODIGOBARRA");

            DataColumn ColPrecioVentaBruto;
            ColPrecioVentaBruto = new DataColumn("PRECIOVENTABRUTO");

            DataColumn colCantidadVenta;
            colCantidadVenta = new DataColumn("CANTIDADVENTA");

            DataColumn colDescripcionProducto;
            colDescripcionProducto = new DataColumn("DESCRIPCIONPRODUCTO");

            detalles.Columns.Add(ColLinea);
            detalles.Columns.Add(ColCodigo);
            detalles.Columns.Add(ColPrecioVentaBruto);
            detalles.Columns.Add(colCantidadVenta);
            detalles.Columns.Add(colDescripcionProducto);

            //"CODIGOBARRA"
            //"PRECIOVENTABRUTO"
            //"CANTIDADVENTA"
            //"DESCRIPCIONPRODUCTO"

        }
        public void CargarDataTable()
        {
            for (int i = 0; i < 3; i++)
            {
                DataRow row1 = detalles.NewRow();
                row1["LINEA"] = i;
                row1["CODIGOBARRA"] = ("Producto " + i);
                row1["PRECIOVENTABRUTO"] = (1000 * i);
                row1["CANTIDADVENTA"] =  i;
                row1["DESCRIPCIONPRODUCTO"] = ("Descripcion "+ i);
                detalles.Rows.Add(row1);
            }
            ////DataRow row1 = detalles.NewRow();
            ////row1["LINEA"] = "value1";
            ////row1["CODIGOBARRA"] = "value2";
            ////row1["PRECIOVENTABRUTO"] = "value2";
            ////row1["CANTIDADVENTA"] = "value2";
            ////row1["DESCRIPCIONPRODUCTO"] = "value2";
            ////detalles.Rows.Add(row1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArmarDataTable();
            CargarDataTable();
            ImprimirTicket("Microsoft Print to PDF", 0, detalles);
        }
        private void ImprimirTicket(string NombreImpresora, int candecimales, DataTable dt)
        {
            TicketCS.Ticket Ticket = new TicketCS.Ticket();
            Decimal importe, cantidad, precio;

            importe = 0;
            cantidad = 0;
            precio = 0;
            
            if (candecimales != 0)
            {
                candecimales = 2;
            }

            try
            {

                //###########################
                //Cabecera
                String nomFantasia, Propietaria, rucEmpresa, timbrado, validez, desvalidez;

                nomFantasia = "Empresa de Prueba";
                Propietaria = "Propietario de Prueba";
                rucEmpresa = "R.U.C. 123456789";
                timbrado = "NRO.TIMBRADO 123456";
                desvalidez = "DESDE 01/01/2021";
                validez = "HASTA 31/12/2021";

                Ticket.AddHeaderLine(nomFantasia);
                Ticket.AddHeaderLine(Propietaria);
                Ticket.AddHeaderLine(rucEmpresa);

                Ticket.AddHeaderLine("");
                Ticket.AddHeaderLine(timbrado);
                Ticket.AddHeaderLine(desvalidez);
                Ticket.AddHeaderLine(validez);
                //##########################
                Ticket.AddHeaderLine("");


                string codigo = "";
                long contador = 0;
                string descripcion = "";

                //##########################
                //Detalles del comprobante
                foreach (DataRow dr in dt.Rows)
                {
                    contador += 1;
                    codigo = dr["CODIGOBARRA"].ToString();
                    precio = Convert.ToDecimal(dr["PRECIOVENTABRUTO"].ToString());
                    importe = Math.Round(Convert.ToDecimal(dr["CANTIDADVENTA"].ToString()) * Convert.ToDecimal(dr["PRECIOVENTABRUTO"].ToString()));
                    descripcion = dr["DESCRIPCIONPRODUCTO"].ToString();
                    cantidad = Convert.ToDecimal(dr["CANTIDADVENTA"].ToString());
                    Ticket.AddItem(codigo, "", descripcion);

                    Ticket.AddItem(cantidad.ToString().PadLeft(13), "", precio.ToString().PadLeft(9) + importe.ToString().PadLeft(10));

                }
                //##########################   


                Ticket.AddHeaderLine(RecortaCaracteres("F A C T U R A  C O N T A D O"));

                Ticket.AddSubHeaderLine("FECHA   : 17/06/2021");
                Ticket.AddSubHeaderLine("NUMERO  : 123456789");
                Ticket.AddSubHeaderLine("CLIENTE : Cliente de Prueba");

                Ticket.AddSubHeaderLine("RUC/CI  : 123456789");


                Ticket.AddSubHeaderLine("DIR.    : ");
                Ticket.AddSubHeaderLine("TEL.    : ");

                Ticket.AddTotal("TOTAL A PAGAR GS ", "0");
                Ticket.AddFooterLine("");

                Ticket.AddTotal(RecortaCaracteres("-----------------------------------"), "");

                Ticket.AddTotal("TOTAL EXENTA GS ", "0");
                Ticket.AddTotal("Gravada 5% GS ", "0");
                Ticket.AddTotal("Gravada 10% GS ", "0");

                Ticket.AddTotal("-----------------------------------", "");

                Ticket.AddTotal("TOTAL I.V.A. 10% ", "0");
                Ticket.AddTotal("TOTAL I.V.A. 5% ", "0");
                Ticket.AddTotal("TOTAL I.V.A. ", "0");

                //############################################
                //Pie del comprobante
                Ticket.AddTotal(RecortaCaracteres("-----------------------------------"),"");

                Ticket.AddFooterLine("");
                Ticket.AddFooterLine(RecortaCaracteres("*GRACIAS POR SU PREFERENCIA*"));
                Ticket.AddFooterLine("");
                Ticket.AddFooterLine(RecortaCaracteres("* * * * *"));
                Ticket.AddFooterLine("");
                //############################################
                    
                if (Ticket.PrinterExists(NombreImpresora) == true )    
                {
                    Ticket.PrintTicket(NombreImpresora);
                }
                
            }
            catch 
            {
               
            }
        }

        private string RecortaCaracteres(string caracter, int longitud = 32)
            {
            if (caracter.Length > longitud)
            {
                caracter = caracter.Substring(0, 32);
            }
            else if (caracter.Length < longitud)
            {
                int longitudEspacio;
                longitudEspacio = ((longitud - caracter.Length) / 2) + caracter.Length;
                caracter = caracter.PadLeft(longitudEspacio);
            }
            return caracter;
        }

    }
}
