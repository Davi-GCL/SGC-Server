using System;
using System.Collections.Generic;

namespace sisteminha.banco;

public class Contas
{
 	int Codconta { get; set; }
	string Agencia { get; set; }
	string? Senha { get; set; }
	decimal Saldo { get; set; }
	byte Tipo { get; set; }
	int? Idusuario { get; set; }
 
}

