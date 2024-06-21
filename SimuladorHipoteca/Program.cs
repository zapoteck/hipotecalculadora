using SimuladorHipoteca;
using System.Data;

double costeTotalVivienda = 137000;
double costeMuebles = 0;
double comunidadMes = 50;
double interes = 2.7;
double inversionInicial = costeTotalVivienda * 0.1;
double gastosImpuestos = costeTotalVivienda * 0.08;
int numeroAños = 30;
double amortizacionMes = 500;

void mostrarDatosHipoteca(string tipo, DataTable dataHipoteca)
{
    Console.WriteLine("\n--------------" + tipo + "\n");
    Console.WriteLine(
        "Año".PadRight(3) + 
        "Cap Pend".PadLeft(12) + 
        "Cuota M".PadLeft(12) + 
        "Amort M".PadLeft(12) + 
        "Intereses".PadLeft(12) + 
        "Cap Amort".PadLeft(12) + 
        "Int Mes".PadLeft(12) + 
        "Cap Amort M".PadLeft(12)
        );
    foreach (DataRow dr in dataHipoteca.Rows)
    {
        Console.WriteLine(
            dr["Año"].ToString().PadRight(3) +
            dr["Capital Pendiente"].ToString().PadLeft(12) +
            dr["Cuota Mensual"].ToString().PadLeft(12) +
            dr["Amortización Mensual"].ToString().PadLeft(12) +
            dr["Intereses"].ToString().PadLeft(12) +
            dr["Capital Amortizado"].ToString().PadLeft(12) +
            dr["Interes Mes"].ToString().PadLeft(12) +
            dr["Capital Amortizado Mes"].ToString().PadLeft(12));
    }
}

Hipoteca h1 = new Hipoteca(costeTotalVivienda, inversionInicial, costeMuebles, comunidadMes, gastosImpuestos, interes, costeTotalVivienda - inversionInicial, numeroAños, amortizacionMes);

mostrarDatosHipoteca("Hipoteca Sin amortizacion", h1.SimularHipoteca());
mostrarDatosHipoteca("Hipoteca con amortización regular de " + amortizacionMes.ToString() + " euros", h1.SimularHipoteca(true));
mostrarDatosHipoteca("Hipoteca con amortización compensada de " + amortizacionMes.ToString() + " euros", h1.SimularHipoteca(true, true));