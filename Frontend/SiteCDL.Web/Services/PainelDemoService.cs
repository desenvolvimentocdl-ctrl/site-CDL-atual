using System.Security.Cryptography;
using System.Text;

namespace SiteCDL.Services
{
    /// <summary>
    /// Painel do Associado — DEMONSTRAÇÃO.
    /// Um único associado de teste com dados de exemplo (nada vem do banco nem de sistemas reais,
    /// exceto cursos/eventos e comunicados, que o painel busca das APIs do site).
    /// Para ligar num sistema real, troque esta classe por uma que consulte a base de associados.
    /// </summary>
    public class PainelDemoService
    {
        public const string LoginDemo = "demo@cdludi.org.br";
        public const string CnpjDemo = "12.345.678/0001-90";
        public const string SenhaDemo = "cdl2026";

        public bool Validar(string? login, string? senha)
        {
            var l = (login ?? "").Trim().ToLowerInvariant();
            var soDigitos = new string(l.Where(char.IsDigit).ToArray());
            var loginOk = l == LoginDemo || (soDigitos.Length == 14 && soDigitos == new string(CnpjDemo.Where(char.IsDigit).ToArray()));
            // comparação em tempo constante, mesmo sendo demonstração
            var senhaOk = CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(senha ?? ""), Encoding.UTF8.GetBytes(SenhaDemo));
            return loginOk && senhaOk;
        }

        public Associado Dados() => new()
        {
            Empresa = "Loja Exemplo Comércio Ltda.",
            NomeFantasia = "Loja Exemplo",
            Cnpj = CnpjDemo,
            Responsavel = "Maria Souza",
            Email = LoginDemo,
            Telefone = "(34) 3239-3400",
            Endereco = "Av. Exemplo, 1000 — Centro, Uberlândia/MG",
            Segmento = "Vestuário e acessórios",
            Plano = "Associado Ouro",
            AssociadoDesde = new DateTime(2015, 3, 12),
            Consultor = "Equipe de Relacionamento CDL",
            ConsultorWhatsapp = "553432393457",
            Spc = new PacoteSpc { Contratadas = 200, Usadas = 128, Renovacao = PrimeiroDiaProximoMes() },
            UltimasConsultas = new()
            {
                new("CPF", "***.482.***-10", DateTime.Today.AddDays(-1), "Nada consta"),
                new("CPF", "***.115.***-77", DateTime.Today.AddDays(-2), "Possui registro"),
                new("CNPJ", "**.903.***/0001-**", DateTime.Today.AddDays(-4), "Nada consta"),
                new("CPF", "***.660.***-03", DateTime.Today.AddDays(-6), "Nada consta"),
            },
            Boletos = MontarBoletos(),
            Beneficios = new()
            {
                new("Orientação jurídica", "Atendimento com advogados parceiros em direito empresarial, trabalhista e do consumidor.", "/vantagens-associado", "Jurídico"),
                new("Consultas SPC com preço de associado", "Pacote mensal de consultas SPC e acesso ao SPC Avisa e Cadastro Positivo.", "/spc-consulta-registro", "SPC"),
                new("Rede de descontos", "Descontos exclusivos em educação, saúde, alimentação e serviços para você e sua equipe.", "/loja/descontos", "Descontos"),
                new("Cursos e treinamentos", "Condições especiais em cursos presenciais, EAD e treinamentos in company.", "/loja/cursos", "Educação"),
                new("Certificado digital", "Emissão de e-CNPJ e e-CPF com valores diferenciados.", "/certificado-digital", "Certificação"),
                new("Salas e auditório", "Locação de auditório e salas de reunião com desconto para associados.", "/cdl-convencoes-e-eventos", "Espaços"),
                new("CDL Saúde", "Planos de saúde coletivos empresariais com condições negociadas pela CDL.", "/cdl-saude", "Saúde"),
                new("CDL Mídia", "Divulgue sua empresa nos canais da CDL e alcance novos clientes.", "/cdl-midia", "Marketing"),
            },
            Inscricoes = new()
            {
                new("Operação Encantamento: do Atendimento ao Sucesso do Cliente", "Curso", DateTime.Today.AddDays(5), "Confirmada", null),
                new("Feira de Oportunidades — 2ª edição", "Evento", DateTime.Today.AddDays(1), "Confirmada", null),
                new("Excel para o Varejo", "Curso", DateTime.Today.AddMonths(-2), "Concluído", "certificado-excel-varejo.pdf"),
                new("Técnicas de Vendas", "Curso", DateTime.Today.AddMonths(-5), "Concluído", "certificado-tecnicas-de-vendas.pdf"),
            },
            Usuarios = new()
            {
                new("Maria Souza", "demo@cdludi.org.br", "Administradora"),
                new("João Pereira", "financeiro@lojaexemplo.com.br", "Financeiro"),
                new("Ana Lima", "spc@lojaexemplo.com.br", "Consultas SPC"),
            },
            Chamados = new()
            {
                new("CH-2026-0187", "Dúvida sobre consulta SPC", DateTime.Today.AddDays(-3), "Respondido"),
                new("CH-2026-0142", "Alteração de endereço de cobrança", DateTime.Today.AddDays(-20), "Concluído"),
            },
        };

        private static DateTime PrimeiroDiaProximoMes() { var h = DateTime.Today; return new DateTime(h.Year, h.Month, 1).AddMonths(1); }

        private static List<Boleto> MontarBoletos()
        {
            var lista = new List<Boleto>();
            var hoje = DateTime.Today;
            for (int i = -4; i <= 1; i++)
            {
                var venc = new DateTime(hoje.Year, hoje.Month, 10).AddMonths(i);
                var status = i < 0 ? "Pago" : (venc < hoje ? "Vencido" : "Em aberto");
                lista.Add(new Boleto(
                    $"Mensalidade {venc:MM/yyyy}", venc, 189.90m, status,
                    // linha digitável fictícia (demonstração)
                    status == "Pago" ? null : $"00190.00009 01234.567890 12345.6789{venc.Month:00}1 1 {venc:yyMM}0000018990"));
            }
            lista.Reverse();
            return lista;
        }

        public class Associado
        {
            public string Empresa = "", NomeFantasia = "", Cnpj = "", Responsavel = "", Email = "", Telefone = "", Endereco = "", Segmento = "", Plano = "", Consultor = "", ConsultorWhatsapp = "";
            public DateTime AssociadoDesde;
            public PacoteSpc Spc = new();
            public List<ConsultaSpc> UltimasConsultas = new();
            public List<Boleto> Boletos = new();
            public List<Beneficio> Beneficios = new();
            public List<Inscricao> Inscricoes = new();
            public List<Usuario> Usuarios = new();
            public List<Chamado> Chamados = new();
        }
        public class PacoteSpc { public int Contratadas, Usadas; public DateTime Renovacao; public int Restantes => Math.Max(0, Contratadas - Usadas); public int Percentual => Contratadas == 0 ? 0 : (int)Math.Round(100.0 * Usadas / Contratadas); }
        public record ConsultaSpc(string Tipo, string Documento, DateTime Data, string Resultado);
        public record Boleto(string Descricao, DateTime Vencimento, decimal Valor, string Status, string? LinhaDigitavel);
        public record Beneficio(string Titulo, string Descricao, string Link, string Categoria);
        public record Inscricao(string Titulo, string Tipo, DateTime Data, string Situacao, string? Certificado);
        public record Usuario(string Nome, string Email, string Perfil);
        public record Chamado(string Protocolo, string Assunto, DateTime Abertura, string Situacao);
    }
}
