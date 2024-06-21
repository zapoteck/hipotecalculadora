using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorHipoteca
{
    public class Hipoteca
    {
        public double CosteTotaVivienda { get; }
        public double InversionInicial { get; }
        public double CosteMuebles { get; }
        public double ComunidadMes { get; }
        public double IBIAnual { get; }
        public double GastosImpuestos { get; }
        public double Interes { get; }
        public double Prestamo { get; }
        public int NumeroAños { get; }
        public double AmortizacionMes { get; }

        double costeMueblesDefecto = 10000;
        double comunidadMesDefecto = 80;
        double interesDefecto = 2.7;
        double multiplicadorInversionInicial = 0.1;
        double multiplicadorGastosImpuestos = 0.08;
        int numeroAñosDefecto = 30;
        double amortizacionMesDefecto = 0;

        public Hipoteca(double pCosteTotalVivienda)
        {
            new Hipoteca(pCosteTotalVivienda, pCosteTotalVivienda * multiplicadorInversionInicial, costeMueblesDefecto, comunidadMesDefecto, 
                CosteTotaVivienda * multiplicadorGastosImpuestos, interesDefecto, CosteTotaVivienda - InversionInicial, numeroAñosDefecto,
                amortizacionMesDefecto);
        }

        public Hipoteca(double pCosteTotalVivienda, double pInversionInicial, double pCosteMuebles, double pComunidadMes, double pGastosImpuestos, double pInteres, double pPrestamo,
            int pNumeroAños, double pAmortizacionMes)
        {
            CosteTotaVivienda = pCosteTotalVivienda;
            InversionInicial = pInversionInicial;
            CosteMuebles = pCosteMuebles;
            ComunidadMes = pComunidadMes;
            GastosImpuestos = pGastosImpuestos;
            Interes = pInteres;
            Prestamo = pPrestamo;
            NumeroAños = pNumeroAños;
            AmortizacionMes = pAmortizacionMes;
        }

        public DataTable SimularHipoteca(bool pAmortizar = false, bool pRegularizarAmortizacion = false)
        {
            double capitalPendiente = Prestamo;
            double cuotaMensual = 0;
            double intereses = 0;
            double capitalAmortizado = 0;
            double interesMes = Interes * 0.01 / 12;
            double capitalAmortizadoMes = 0;
            double cantidadAmortizar = (pAmortizar ? AmortizacionMes : 0);
            double cantidadTotalMensual = 0;

            DataTable datosHipoteca = new DataTable();

            datosHipoteca.Columns.Add("Año");
            datosHipoteca.Columns.Add("Capital Pendiente");
            datosHipoteca.Columns.Add("Cuota Mensual");
            datosHipoteca.Columns.Add("Amortización Mensual");
            datosHipoteca.Columns.Add("Intereses");
            datosHipoteca.Columns.Add("Capital Amortizado");
            datosHipoteca.Columns.Add("Interes Mes");
            datosHipoteca.Columns.Add("Capital Amortizado Mes");

            for (int i = 1; i <= NumeroAños && capitalPendiente - capitalAmortizado - cantidadAmortizar * 12 > 0; i++)
            {
                double amortizacionTotal = (cantidadAmortizar * 12 * (i - 1));
                cuotaMensual = ((Prestamo - amortizacionTotal) * Interes * 0.01) / (12 * (1 - Math.Pow((1 + Interes * 0.01 / 12), -12 * NumeroAños)));
                if (i != 1) capitalPendiente = capitalPendiente - capitalAmortizado - cantidadAmortizar * 12;
                else cantidadTotalMensual = cuotaMensual + AmortizacionMes;
                intereses = capitalPendiente * Interes * 0.01;
                capitalAmortizado = cuotaMensual * 12 - intereses;
                interesMes = intereses / 12;
                capitalAmortizadoMes = capitalAmortizado / 12;
                if (pRegularizarAmortizacion) cantidadAmortizar = cantidadTotalMensual - cuotaMensual;

                //Redondeamos
                cuotaMensual = Math.Round(cuotaMensual, 2);
                capitalPendiente = Math.Round(capitalPendiente, 2);
                capitalAmortizado = Math.Round(capitalAmortizado, 2);
                capitalAmortizadoMes = Math.Round(capitalAmortizadoMes, 2);
                intereses = Math.Round(intereses, 2);
                interesMes = Math.Round(interesMes, 2);
                cantidadAmortizar = Math.Round(cantidadAmortizar, 2);

                datosHipoteca.Rows.Add(i, capitalPendiente, cuotaMensual, cantidadAmortizar, intereses, capitalAmortizado, interesMes, capitalAmortizado); 
            }

            return datosHipoteca;
        }
    }
}
