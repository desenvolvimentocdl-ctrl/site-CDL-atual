namespace SiteCDL.Services
{
    /// <summary>
    /// GEO (Generative Engine Optimization): lista das páginas fixas do site, com uma descrição curta de cada uma.
    /// Usada para montar /llms.txt (resumo do site para ChatGPT, Claude, Perplexity, Gemini etc.) e /sitemap.xml.
    /// Ao criar ou remover uma página em Views/Paginas ou na Loja, atualize esta lista.
    /// </summary>
    public static class GeoCatalogo
    {
        /// <summary>Endereço oficial do site; os links de /llms.txt, /sitemap.xml e da tag canonical sempre apontam para ele.</summary>
        public const string SiteUrlPadrao = "https://cdludi.org.br";

        public record Pagina(string Caminho, string Titulo, string Descricao);
        public record Grupo(string Nome, Pagina[] Paginas);

        public const string Resumo =
            "A CDL Uberlândia (Câmara de Dirigentes Lojistas de Uberlândia) é uma associação privada sem fins lucrativos, " +
            "fundada em 1977 e integrante do Sistema CNDL, que representa e apoia o comércio, os serviços e o setor produtivo de Uberlândia (MG). " +
            "Oferece aos associados consultas e registros no SPC Brasil, certificado digital, plano de saúde, cursos, eventos, " +
            "locação de salas e auditório, recrutamento, programas de aprendiz e estágio, além de representatividade junto ao poder público.";

        public static readonly Grupo[] Grupos =
        {
            new("Institucional", new Pagina[]
            {
                new("/quem-somos", "Quem somos", "Missão e atuação da CDL Uberlândia em defesa do setor produtivo, da atração de novos negócios e da qualidade de vida do município."),
                new("/nossa-historia", "Nossa História", "Linha do tempo da CDL Uberlândia desde a fundação, em 1977, e do SPC."),
                new("/diretoria", "Diretoria CDL", "Composição da diretoria da CDL Uberlândia na gestão 2025/2026."),
                new("/governanca", "Governança", "Estrutura de governança e transparência da entidade."),
                new("/representatividade", "Representatividade", "Atuação da CDL Uberlândia junto ao poder público e a outras entidades em nome do comércio."),
                new("/reconhecimento", "Reconhecimento", "Certificações e selos recebidos pela CDL em sustentabilidade, inclusão social e governança, como o selo Juntos pelos ODS (2024)."),
                new("/fundacao-cdl", "Fundação CDL", "Fundação CDL (FUNDECS), braço social e educacional da CDL Uberlândia."),
                new("/diretoria-fundacao", "Diretoria Fundação", "Diretoria da Fundação CDL."),
                new("/responsabilidade-social", "Responsabilidade Social", "Ações sociais promovidas pela CDL Uberlândia."),
                new("/responsabilidade-ambiental", "Responsabilidade Ambiental", "Iniciativas ambientais da CDL Uberlândia."),
                new("/politica-de-sustentabilidade", "Política de Sustentabilidade", "Política de sustentabilidade adotada pela entidade."),
                new("/lgpd", "Área LGPD", "Termos de Uso e Política de Privacidade da CDL Uberlândia e da Fundação CDL."),
                new("/licitacoes", "Licitações", "Processos licitatórios do Município de Uberlândia e da Universidade Federal de Uberlândia."),
            }),
            new("Associação e vantagens", new Pagina[]
            {
                new("/associe-se", "Associe-se", "Como se associar à CDL Uberlândia e o que a empresa recebe ao se tornar associada."),
                new("/vantagens-associado", "Vantagens para Associados", "Benefícios, descontos e serviços exclusivos para empresas associadas."),
                new("/combo-solucoes", "Combo de Soluções", "Pacote de soluções exclusivas para associados, com investimento e serviços incluídos."),
                new("/corporativo", "Corporativo", "A CDL Uberlândia dentro do Sistema CNDL (CNDL, FCDL-MG, SPC Brasil) e sua estrutura corporativa."),
                new("/bdmg", "Parceria BDMG", "Parceria com o Banco de Desenvolvimento de Minas Gerais (BDMG) para crédito a empresas."),
            }),
            new("SPC Brasil e crédito", new Pagina[]
            {
                new("/spc-brasil", "SPC Brasil", "O que é o SPC Brasil e como a CDL Uberlândia oferece seus serviços às empresas."),
                new("/spc-consulta", "SPC Consulta", "Como saber se o seu nome está no SPC: consulta gratuita na sede da CDL Uberlândia ou pela loja online do SPC Brasil."),
                new("/spc-consulta-registro", "SPC Consultas e Registro", "Consultas de crédito e registro de inadimplentes no SPC para empresas."),
                new("/spc-negociacao", "SPC Negociação de Débito", "Como negociar débitos registrados no SPC."),
                new("/spc-avisa", "SPC Avisa", "Monitoramento do CPF ou CNPJ: aviso por e-mail ou SMS quando houver consulta, registro ou abertura de empresa em seu nome."),
                new("/spc-dados", "SPC Dados", "Atualização, padronização e enriquecimento do cadastro de clientes da empresa."),
                new("/cadastro-positivo", "Cadastro Positivo", "Informações sobre o Cadastro Positivo e como ele afeta o crédito."),
                new("/alerta-documento", "Alerta de Documentos", "Serviço gratuito de utilidade pública que informa às empresas associadas sobre documentos furtados, roubados ou extraviados, prevenindo fraudes."),
            }),
            new("Serviços para empresas", new Pagina[]
            {
                new("/certificado-digital", "Certificado Digital", "Emissão de certificado digital (e-CPF e e-CNPJ) pela CDL Uberlândia."),
                new("/cdl-saude", "CDL Saúde", "Plano de saúde para associados, colaboradores e seus dependentes."),
                new("/cdl-gestao", "CDL Gestão e NF-e", "Sistema de gestão e emissão de nota fiscal eletrônica."),
                new("/cdl-midia", "CDL Mídia", "Divulgação da empresa nas redes sociais e no app da CDL (com cashback), rede de descontos e webpage."),
                new("/cdl-talentos", "Recrutamento e Seleção", "Serviço de recrutamento e seleção de profissionais para empresas."),
                new("/cdl-aprendiz", "CDL Aprendiz", "Programa Jovem Aprendiz da CDL Uberlândia."),
                new("/cdl-estagio", "CDL Estágio", "Programa de estágio e intermediação entre empresas e estudantes."),
                new("/treinamento", "Treinamento in company", "Treinamentos personalizados realizados dentro da empresa."),
                new("/reforma-tributaria", "Reforma Tributária", "Guia da Reforma Tributária do Consumo para o comércio: IBS, CBS e Imposto Seletivo, novidades, cronograma 2026–2033, Simples Nacional, split payment e links oficiais."),
                new("/giro-mercado", "Giro de Mercado", "Evento gratuito em parceria com o SEBRAE, com mesas temáticas e consultores, para gerar negócios entre empresários."),
                new("/legislacao", "Legislação em pauta", "Orientações jurídicas para empresas, como as principais dúvidas sobre atestado médico."),
            }),
            new("Eventos e espaços", new Pagina[]
            {
                new("/cdl-convencoes-e-eventos", "CDL Convenções e Eventos", "Centro de convenções da CDL Uberlândia para eventos corporativos."),
                new("/cdl-auditorio", "CDL Auditório", "Auditório para até 448 pessoas, com palco de 50 m², para palestras, convenções e eventos."),
                new("/cdl-foyer", "CDL Foyer", "Foyer para coquetéis, recepções e exposições."),
                new("/sala-executiva", "Sala Executiva", "Locação de sala para reuniões executivas."),
                new("/sala-cecilia-meireles", "Sala Cecília Meireles", "Locação de sala para até 60 pessoas: cursos, reuniões e palestras."),
                new("/sala-machado-de-assis", "Sala Machado de Assis", "Locação de sala para até 20 pessoas: treinamentos e reuniões."),
                new("/sala-espaco-mais", "Sala Espaço Mais", "Locação de espaço para até 120 pessoas, com palco: networking, lançamentos e eventos sociais."),
                new("/cafe-empreendedor", "Café Empreendedor", "Evento aberto ao público sobre empreendedorismo, negócios, tecnologia e inovação, sediado a cada edição por uma empresa associada."),
                new("/jornada-empresarial", "Jornada Empresarial", "Programa gratuito de trilhas de gestão e networking para empresários associados."),
                new("/feira-de-oportunidade", "Feira de Oportunidades", "Feira de negócios com mais de 300 expositores para prospecção de clientes e parceiros."),
            }),
            new("Loja CDL (cursos, eventos e descontos)", new Pagina[]
            {
                new("/loja", "Loja CDL", "Agenda de cursos e eventos com inscrição online."),
                new("/loja/cursos", "Cursos presenciais", "Cursos presenciais oferecidos pela CDL Uberlândia."),
                new("/loja/cursos-ead", "Cursos EAD", "Cursos a distância (EAD) disponíveis para associados e público geral."),
                new("/loja/eventos", "Eventos", "Próximos eventos da CDL Uberlândia."),
                new("/loja/descontos", "Descontos", "Descontos oferecidos por parceiros aos associados."),
                new("/loja/parceiros", "Parceiros", "Empresas parceiras da CDL Uberlândia."),
                new("/loja/menor-preco", "CDL Menor Preço", "Plataforma CDL MenorPreço: solicitações de orçamento abertas para fornecedores associados."),
            }),
            new("Temas e redes sociais", new Pagina[]
            {
                new("/vender-mais", "Vender mais", "Soluções para divulgar a empresa e atrair clientes: CDL Mídia, SPC Dados e Rede de Descontos."),
                new("/gestao-e-economia", "Gestão e economia", "Certificado Digital, CDL Gestão e NF-e, CDL Saúde, Parceria BDMG, Combo de Soluções e Consultoria."),
                new("/pessoas", "Pessoas", "Recrutamento e Seleção, CDL Estágio e CDL Aprendiz."),
                new("/spc-para-empresas", "SPC para sua empresa", "SPC Consultas e Registro, SPC Avisa, SPC Dados e Cadastro Positivo."),
                new("/spc-para-voce", "SPC para você", "SPC Consulta CPF, SPC Negociação de Débito e Alerta de Documentos."),
                new("/aprender", "Aprender", "Eventos, cursos presenciais, cursos EAD e treinamento in company."),
                new("/economizar", "Economizar", "Descontos, parceiros e a plataforma CDL MenorPreço."),
                new("/solucoes", "Soluções", "Todas as soluções da CDL Uberlândia para empresas, por grupo: vender mais, gestão e economia, pessoas e espaços para eventos."),
                new("/cursos-e-eventos", "Cursos e Eventos", "Cursos, eventos, treinamentos, descontos, parceiros e CDL MenorPreço."),
                new("/a-cdl", "A CDL", "Páginas institucionais da CDL Uberlândia: quem somos, história, Fundação CDL, diretoria, sustentabilidade e licitações."),
                new("/redes-sociais", "Redes sociais", "Instagram, Facebook, YouTube, LinkedIn, X e WhatsApp da CDL Uberlândia, com os vídeos mais recentes do canal."),
            }),
            new("Conteúdo", new Pagina[]
            {
                new("/publicacoes", "Todas as publicações", "Notícias, comunicados e pesquisas mais recentes da CDL Uberlândia."),
                new("/publicacoes/noticias", "Notícias", "Notícias sobre o comércio de Uberlândia e a atuação da CDL."),
                new("/publicacoes/comunicados", "Comunicados", "Comunicados oficiais aos associados."),
                new("/publicacoes/pesquisas", "Pesquisas", "Pesquisas de mercado e de intenção de compra realizadas pela CDL."),
                new("/publicacoes/cdlacao", "CDL em Ação", "Ações e projetos realizados pela CDL Uberlândia."),
                new("/publicacoes/cdldireto", "Opinião", "Artigos de opinião."),
                new("/publicacoes/cae", "CAE", "Publicações da categoria CAE."),
            }),
        };
    }
}
