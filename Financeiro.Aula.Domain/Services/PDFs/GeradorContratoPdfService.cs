using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Interfaces.Services.PDFs;
using Financeiro.Aula.Domain.ValueObjects;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Financeiro.Aula.Domain.Services.PDFs
{
    public class GeradorContratoPdfService : IGeradorContratoPdfService
    {
        private const int TAMANHO_FONTE = 12;

        private readonly IEmpresaRepository _empresaRepository;

        public GeradorContratoPdfService(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }

        public async Task<byte[]?> GerarContratoMatricula(Contrato contrato)
        {
            var empresa = await _empresaRepository.ObterEmpresaPadrao();

            if (empresa is null)
                return null;

            if (contrato.Cliente is null)
                return null;

            var termoLinhas = GerarTermosLinhas();

            //
            // gera o PDF
            Rectangle rec = CriaRectangle(21M, 29.7M);

            var stream = new MemoryStream();

            Document documento = new Document(rec, Cm2Points(0.5f), Cm2Points(0.5f), Cm2Points(0.5f), Cm2Points(0.5f));
            PdfWriter writer = PdfWriter.GetInstance(documento, stream);

            documento.AddCreator(empresa.RazaoSocial);
            documento.AddTitle("Contrato da matrícula");

            //
            // preenche os dados no PDF
            documento.Open();

            Paragraph paragrafo;

            paragrafo = new Paragraph($"CONTRATO DE PRESTAÇÃO DE SERVIÇOS - NRO.: {contrato.Id}", GetFont(14, true));
            paragrafo.Alignment = Element.ALIGN_CENTER;
            paragrafo.SpacingAfter = 15f;
            documento.Add(paragrafo);

            //
            // CONTRATANTE
            PdfPTable tabelaResponsavel = new PdfPTable(3);
            tabelaResponsavel.DefaultCell.Border = 0;
            tabelaResponsavel.SetWidths(new float[] { 1F, 0.75F, 1F });
            tabelaResponsavel.SpacingAfter = 15f;
            tabelaResponsavel.WidthPercentage = 100f;
            tabelaResponsavel.KeepTogether = true;

            tabelaResponsavel.AddCell(GeraCelulaDados("QUADRO 01. CONTRATANTE", 3));

            tabelaResponsavel.AddCell(GeraCelulaDados("Nome", contrato.Cliente.Nome, 3));
            tabelaResponsavel.AddCell(GeraCelulaDados("CPF", contrato.Cliente.Cpf, 1));

            tabelaResponsavel.AddCell(GeraCelulaDados("Ident.", contrato.Cliente.Identidade, 1));
            tabelaResponsavel.AddCell(GeraCelulaDados("Data nasc.", contrato.Cliente.DataNascimento.ToString("dd/MM/yyyy") ?? string.Empty, 1));

            tabelaResponsavel.AddCell(GeraCelulaDados("Telefones", contrato.Cliente.Telefone, 3));

            tabelaResponsavel.AddCell(GeraCelulaDados("Endereço", $"{contrato.Cliente.Endereco.Logradouro}, {contrato.Cliente.Endereco.Numero}{(!string.IsNullOrEmpty(contrato.Cliente.Endereco.Complemento) ? " - " + contrato.Cliente.Endereco.Complemento : null)}", 3));

            tabelaResponsavel.AddCell(GeraCelulaDados("Bairro", contrato.Cliente.Endereco.Bairro, 1));
            tabelaResponsavel.AddCell(GeraCelulaDados("CEP", contrato.Cliente.Endereco.Cep, 1));
            tabelaResponsavel.AddCell(GeraCelulaDados("Cidade", contrato.Cliente.Endereco.Municipio + "/" + contrato.Cliente.Endereco.Uf, 1));

            documento.Add(tabelaResponsavel);

            //
            // CONTRATADA
            PdfPTable tabelaContratada = new PdfPTable(3);
            tabelaContratada.DefaultCell.Border = 0;
            tabelaContratada.SetWidths(new float[] { 1F, 0.75F, 1F });
            tabelaContratada.SpacingAfter = 15f;
            tabelaContratada.WidthPercentage = 100f;
            tabelaContratada.KeepTogether = true;

            tabelaContratada.AddCell(GeraCelulaDados("QUADRO 03. CONTRATADA", 3));

            tabelaContratada.AddCell(GeraCelulaDados("Empresa", empresa.RazaoSocial, 3));

            tabelaContratada.AddCell(GeraCelulaDados("Telefone", empresa.Telefone, 1));
            tabelaContratada.AddCell(GeraCelulaDados("CNPJ", empresa.CNPJ, 2));

            tabelaContratada.AddCell(GeraCelulaDados("Endereço", $"{empresa.Endereco.Logradouro}, {empresa.Endereco.Numero}{(!string.IsNullOrEmpty(empresa.Endereco.Complemento) ? " - " + empresa.Endereco.Complemento : null)}", 3));

            tabelaContratada.AddCell(GeraCelulaDados("Bairro", empresa.Endereco.Bairro, 1));
            tabelaContratada.AddCell(GeraCelulaDados("CEP", empresa.Endereco.Cep, 1));
            tabelaContratada.AddCell(GeraCelulaDados("Cidade", $"{empresa.Endereco.Municipio}/{empresa.Endereco.Uf}", 1));

            documento.Add(tabelaContratada);

            //
            // CURSO
            PdfPTable tabelaCurso = new PdfPTable(3);
            tabelaCurso.DefaultCell.Border = 0;
            tabelaCurso.SetWidths(new float[] { 0.5F, 1F, 0.5F });
            tabelaCurso.SpacingAfter = 15f;
            tabelaCurso.WidthPercentage = 100f;
            tabelaCurso.KeepTogether = true;

            tabelaCurso.AddCell(GeraCelulaDados("QUADRO 04. CURSO", 3));

            tabelaCurso.AddCell(GeraCelulaDados("Curso", contrato.Turma.Curso.Descricao, 2));
            tabelaCurso.AddCell(GeraCelulaDados("C.H.", contrato.Turma.Curso.CargaHoraria.ToString("f0") + "hrs", 1));
            tabelaCurso.AddCell(GeraCelulaDados("Turma", contrato.Turma.Numero, 1));
            tabelaCurso.AddCell(GeraCelulaDados("Horário", contrato.Turma.Horario, 1));
            tabelaCurso.AddCell(GeraCelulaDados("Prev. início", contrato.Turma.DataInicio.ToString("dd/MM/yyyy"), 1));

            documento.Add(tabelaCurso);

            //
            // INVESTIMENTO, PARCELAMENTO E TAXA MATRÍCULA
            PdfPTable tabelaInvestimento = new PdfPTable(3);
            tabelaInvestimento.DefaultCell.Border = 0;
            tabelaInvestimento.SetWidths(new float[] { 1F, 1F, 0.5F });
            tabelaInvestimento.SpacingAfter = 15f;
            tabelaInvestimento.WidthPercentage = 100f;
            tabelaInvestimento.KeepTogether = true;

            tabelaInvestimento.AddCell(GeraCelulaDados("QUADRO 05. INVESTIMENTO", 3));

            //
            int numeroColunasTaxaMatriculaEParcelamento = 3;

            var widthsTaxaMatricula = new float[numeroColunasTaxaMatriculaEParcelamento * 3];
            for (int i = 0; i < widthsTaxaMatricula.Length; i++)
            {
                widthsTaxaMatricula[i] = i % numeroColunasTaxaMatriculaEParcelamento == 0 ? 0.5F : 1F;
            }

            tabelaInvestimento.AddCell(GeraCelulaDados("Valor do contrato", contrato.ValorTotal.ToString("c2"), 3));

            documento.Add(tabelaInvestimento);

            // parcelamento
            var widthsParcelamento = new float[numeroColunasTaxaMatriculaEParcelamento * 3];
            for (int i = 0; i < widthsParcelamento.Length; i++)
            {
                widthsParcelamento[i] = i % numeroColunasTaxaMatriculaEParcelamento == 0 ? 0.5F : 1F;
            }

            PdfPTable tabelaParcelamento = new PdfPTable(widthsParcelamento.Length);
            tabelaParcelamento.DefaultCell.Border = 0;
            tabelaParcelamento.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabelaParcelamento.SetWidths(widthsParcelamento);
            tabelaParcelamento.SpacingAfter = 15f;
            tabelaParcelamento.WidthPercentage = 100f;
            tabelaParcelamento.KeepTogether = true;

            tabelaParcelamento.AddCell(GeraCelulaDados("QUADRO 05.01. PARCELAMENTO", widthsParcelamento.Length));

            for (int j = 0; j < numeroColunasTaxaMatriculaEParcelamento; j++)
            {
                tabelaParcelamento.AddCell("");
                tabelaParcelamento.AddCell(new Phrase("Vencimento", GetFont(12, true)));
                tabelaParcelamento.AddCell(new Phrase("Valor", GetFont(12, true)));
            }

            var parcelasImprimir = contrato.Parcelas;

            if (parcelasImprimir != null)
            {
                int numeroLinhasImprimir = (int)Math.Ceiling((decimal)parcelasImprimir.Count / numeroColunasTaxaMatriculaEParcelamento);

                for (int i = 0; i < numeroLinhasImprimir; i++)
                {
                    for (int j = 0; j < numeroColunasTaxaMatriculaEParcelamento; j++)
                    {
                        int ix = i + j * numeroLinhasImprimir;
                        var parcela = parcelasImprimir.Count > ix ? parcelasImprimir.ElementAt(ix) : null;

                        if (parcela != null)
                        {
                            tabelaParcelamento.AddCell(parcela.Sequencial.ToString());
                            tabelaParcelamento.AddCell(parcela.DataVencimento.ToString("dd/MM/yyyy"));
                            tabelaParcelamento.AddCell(parcela.Valor.ToString("c2"));
                        }
                        else
                        {
                            tabelaParcelamento.AddCell("");
                            tabelaParcelamento.AddCell("");
                            tabelaParcelamento.AddCell("");
                        }
                    }
                }
            }

            documento.Add(tabelaParcelamento);

            //
            // CLÁUSULAS CONTRATUAIS
            documento.Add(new Phrase("CLÁUSULAS CONTRATUAIS", GetFont(12, true)));

            foreach (var termoLinha in termoLinhas)
            {
                paragrafo = new Paragraph();
                paragrafo.Alignment = Element.ALIGN_JUSTIFIED;

                paragrafo.Add(new Chunk(termoLinha.Clausula + " - ", GetFont(12, true)));
                paragrafo.Add(new Chunk(termoLinha.Texto, GetFont(12, termoLinha.Negrito)));

                documento.Add(paragrafo);
            }

            //
            //
            paragrafo = new Paragraph();
            paragrafo.Alignment = Element.ALIGN_LEFT;
            paragrafo.SetLeading(0f, 3f);
            paragrafo.Add(new Phrase($"{empresa.Endereco.Municipio}/{empresa.Endereco.Uf}, {contrato.DataEmissao.ToString("dd 'de' MMMM 'de' yyyy")}.", GetFont(12, true)));
            documento.Add(paragrafo);

            PdfPTable tabelaAssinatura = new PdfPTable(2);
            tabelaAssinatura.WidthPercentage = 100f;
            tabelaAssinatura.SpacingBefore = 40f;
            tabelaAssinatura.DefaultCell.Border = 0;
            tabelaAssinatura.DefaultCell.Column.Alignment = Element.ALIGN_CENTER;

            tabelaAssinatura.AddCell(new Phrase(". . . . . Assinado digitalmente . . . . .", GetFont(8, false, true)));
            tabelaAssinatura.AddCell(new Phrase(". . . . . Assinado digitalmente . . . . .", GetFont(8, false, true)));
            tabelaAssinatura.AddCell("CONTRATANTE");
            tabelaAssinatura.AddCell("CONTRATADA");
            
            documento.Add(tabelaAssinatura);

            //
            //
            documento.Close();

            return stream.GetBuffer();
        }

        private List<TermoImpressao> GerarTermosLinhas()
        {
            return new List<TermoImpressao>()
            {
                new TermoImpressao("1", "DAS OBRIGAÇÕES DA CONTRATADA", true, 1),
                new TermoImpressao("1.1", "O incentivo ao avanço tecnológico, assim como a estrutura atual da organização prepara-nos para enfrentar situações atípicas decorrentes das condições inegavelmente apropriadas.", false, 1),
                new TermoImpressao("1.2", "Assim mesmo, a consolidação das estruturas prepara-nos para enfrentar situações atípicas decorrentes dos relacionamentos verticais entre as hierarquias. Neste sentido, a valorização de fatores subjetivos talvez venha a ressaltar a relatividade das regras de conduta normativas.", false, 1),
                new TermoImpressao("1.3", "Todas estas questões, devidamente ponderadas, levantam dúvidas sobre se o desafiador cenário globalizado não pode mais se dissociar dos paradigmas corporativos. Não obstante, o consenso sobre a necessidade de qualificação estende o alcance e a importância dos métodos utilizados na avaliação de resultados.", false, 1),
                new TermoImpressao("1.4", "Todavia, o início da atividade geral de formação de atitudes talvez venha a ressaltar a relatividade do fluxo de informações.", true, 1),
                new TermoImpressao("1.5", "Pensando mais a longo prazo, a percepção das dificuldades afeta positivamente a correta previsão do impacto na agilidade decisória.", false, 1),
                new TermoImpressao("2", "DAS OBRIGAÇÕES DO CONTRATANTE", true, 1),
                new TermoImpressao("2.1", "Todas estas questões, devidamente ponderadas, levantam dúvidas sobre se o aumento do diálogo entre os diferentes setores produtivos prepara-nos para enfrentar situações atípicas decorrentes do sistema de participação geral.", false, 1),
                new TermoImpressao("2.2", "Ainda assim, existem dúvidas a respeito de como o fenômeno da Internet prepara-nos para enfrentar situações atípicas decorrentes dos relacionamentos verticais entre as hierarquias. O cuidado em identificar pontos críticos na determinação clara de objetivos estimula a padronização das posturas dos órgãos dirigentes com relação às suas atribuições.", false, 1),
                new TermoImpressao("2.3", "Por outro lado, o desafiador cenário globalizado facilita a criação do sistema de participação geral. A certificação de metodologias que nos auxiliam a lidar com a constante divulgação das informações cumpre um papel essencial na formulação das condições inegavelmente apropriadas.", false, 1)
            };
        }

        private PdfPCell GeraCelulaDados(string titulo, int colspan)
        {
            var phrase = new Phrase();
            phrase.Add(new Paragraph(titulo, GetFont(TAMANHO_FONTE, true)));

            var cell = new PdfPCell();
            cell.Colspan = colspan;
            cell.Border = 0;
            cell.AddElement(phrase);

            return cell;
        }

        private PdfPCell GeraCelulaDados(string titulo, string texto, int colspan)
        {
            var phrase = new Phrase();

            if (!string.IsNullOrEmpty(titulo) && !string.IsNullOrEmpty(texto))
            {
                phrase.Add(new Paragraph(titulo + ": ", GetFont(TAMANHO_FONTE, true)));
                phrase.Add(new Paragraph(texto, GetFont(TAMANHO_FONTE)));
            }

            var cell = new PdfPCell();
            cell.Colspan = colspan;
            cell.Border = 0;
            cell.AddElement(phrase);

            return cell;
        }

        protected static float Cm2Points(decimal cm)
        {
            return Cm2Points((float)cm);
        }

        protected static float Cm2Points(float cm)
        {
            // 72 points in a inch
            // 1 inch = 2.54 centimeters
            // 72 points = 2.54 cm

            // 2.54cm = 72points
            //     Cm = x
            // x = (72 * Cm) / 2.54

            return (72F * cm) / 2.54F;
        }

        protected static float Points2Cm(float po)
        {
            // 72 points in a inch
            // 1 inch = 2.54 centimeters
            // 72 points = 2.54 cm

            // 2.54cm = 72points
            //      x = Po
            // x = (2.54 * Po) / 72

            return (2.54F * po) / 72F;
        }

        protected static Rectangle CriaRectangle(decimal cmX, decimal cmY)
        {
            return CriaRectangle((float)cmX, (float)cmY);
        }

        protected static Rectangle CriaRectangle(float cmX, float cmY)
        {
            return new Rectangle(Cm2Points(cmX), Cm2Points(cmY));
        }

        protected PdfPCell GeraCelulaDadosPadrao(string texto, int colspan, Font font, int alignment, int border)
        {
            var phrase = new Phrase();
            phrase.Add(new Paragraph(texto, font));

            var cell = new PdfPCell(phrase);
            cell.Colspan = colspan;
            cell.Border = border;
            cell.HorizontalAlignment = alignment;

            return cell;
        }

        protected PdfPTable GeraTabelaTopo()
        {
            //
            PdfPTable tabelaTopo = new PdfPTable(2);
            tabelaTopo.WidthPercentage = 100f;
            tabelaTopo.DefaultCell.Border = 0;

            var caminhoImagemTopo = @"C:\Users\Felipe\Desktop\11885683_996457447043013_3251962389736372515_o.jpg";

            Image? imagemTopoTermos = null;
            if (File.Exists(caminhoImagemTopo))
                imagemTopoTermos = Image.GetInstance(caminhoImagemTopo);

            PdfPCell celulaImagem = new PdfPCell(imagemTopoTermos, true);
            celulaImagem.Border = 0;
            celulaImagem.HorizontalAlignment = Element.ALIGN_CENTER;
            celulaImagem.VerticalAlignment = Element.ALIGN_MIDDLE;
            celulaImagem.PaddingLeft = 20f;
            celulaImagem.PaddingRight = 20f;
            tabelaTopo.AddCell(celulaImagem);

            //
            // CABEÇALHO
            //
            PdfPTable tabelaCabecalho = new PdfPTable(2);
            tabelaCabecalho.SetWidths(new float[] { 1f, 3f });
            tabelaCabecalho.WidthPercentage = 100f;
            tabelaCabecalho.DefaultCell.Border = 0;

            tabelaCabecalho.AddCell(GeraCelulaDadosPadrao("empresa.RazaoSocial", 2, GetFont(10, true), Element.ALIGN_LEFT, Rectangle.NO_BORDER));
            tabelaCabecalho.AddCell(new Phrase("Endereço: ", GetFont(10, true)));
            tabelaCabecalho.AddCell(new Phrase("empresa.Endereco" + ", " + "empresa.NumeroEndereco", GetFont(10)));
            tabelaCabecalho.AddCell(new Phrase("CEP: ", GetFont(10, true)));
            tabelaCabecalho.AddCell(new Phrase("empresa.CEP", GetFont(10)));
            tabelaCabecalho.AddCell(new Phrase("Cidade: ", GetFont(10, true)));
            tabelaCabecalho.AddCell(new Phrase("empresa.Cidade" + "/" + "empresa.Cidade?.UF", GetFont(10)));
            tabelaCabecalho.AddCell(new Phrase("CNPJ: ", GetFont(10, true)));
            tabelaCabecalho.AddCell(new Phrase("empresa.CNPJ", GetFont(10)));
            tabelaCabecalho.AddCell(new Phrase("Fone: ", GetFont(10, true)));
            tabelaCabecalho.AddCell(new Phrase("empresa.Telefone", GetFont(10)));
            tabelaTopo.AddCell(tabelaCabecalho);

            return tabelaTopo;
        }

        protected Font GetFont(int size, bool bold = false, bool sublinhado = false)
        {
            var style = Font.NORMAL;
            if (bold) style += Font.BOLD;
            if (sublinhado) style += Font.UNDERLINE;

            return FontFactory.GetFont("Arial", size, style);
        }
    }
}