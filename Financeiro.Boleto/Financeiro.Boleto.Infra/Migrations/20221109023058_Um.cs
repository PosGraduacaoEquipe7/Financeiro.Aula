using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financeiro.Boleto.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Um : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boletos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChaveBoleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Valor = table.Column<double>(type: "float", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnderecoBairro = table.Column<string>(name: "Endereco_Bairro", type: "nvarchar(max)", nullable: false),
                    EnderecoCep = table.Column<string>(name: "Endereco_Cep", type: "nvarchar(max)", nullable: false),
                    EnderecoComplemento = table.Column<string>(name: "Endereco_Complemento", type: "nvarchar(max)", nullable: true),
                    EnderecoLogradouro = table.Column<string>(name: "Endereco_Logradouro", type: "nvarchar(max)", nullable: false),
                    EnderecoMunicipio = table.Column<string>(name: "Endereco_Municipio", type: "nvarchar(max)", nullable: false),
                    EnderecoNumero = table.Column<string>(name: "Endereco_Numero", type: "nvarchar(max)", nullable: false),
                    EnderecoUf = table.Column<string>(name: "Endereco_Uf", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boletos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParametrosBoleto",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Agencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroConta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Carteira = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroBoletoAtual = table.Column<int>(type: "int", nullable: false),
                    NomeBeneficiario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CnpjBeneficiario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnderecoBeneficiarioBairro = table.Column<string>(name: "EnderecoBeneficiario_Bairro", type: "nvarchar(max)", nullable: false),
                    EnderecoBeneficiarioCep = table.Column<string>(name: "EnderecoBeneficiario_Cep", type: "nvarchar(max)", nullable: false),
                    EnderecoBeneficiarioComplemento = table.Column<string>(name: "EnderecoBeneficiario_Complemento", type: "nvarchar(max)", nullable: true),
                    EnderecoBeneficiarioLogradouro = table.Column<string>(name: "EnderecoBeneficiario_Logradouro", type: "nvarchar(max)", nullable: false),
                    EnderecoBeneficiarioMunicipio = table.Column<string>(name: "EnderecoBeneficiario_Municipio", type: "nvarchar(max)", nullable: false),
                    EnderecoBeneficiarioNumero = table.Column<string>(name: "EnderecoBeneficiario_Numero", type: "nvarchar(max)", nullable: false),
                    EnderecoBeneficiarioUf = table.Column<string>(name: "EnderecoBeneficiario_Uf", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametrosBoleto", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Boletos");

            migrationBuilder.DropTable(
                name: "ParametrosBoleto");
        }
    }
}
