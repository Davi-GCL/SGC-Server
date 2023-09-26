using System;
using System.Collections.Generic;

namespace sistemabanco;

public class Mov
{
 	int Idmov { get; set; }
	int Idconta { get; set; }
	DateTime? Datahora { get; set; }
	decimal Valor { get; set; }
	string Tipo { get; set; }
 
}

