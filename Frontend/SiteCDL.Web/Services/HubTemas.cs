namespace SiteCDL.Services
{
    /// <summary>
    /// Páginas de tema do menu (ex.: /vender-mais): cada título azul dos grupos do menu leva a uma página que reúne
    /// os itens daquele grupo. As descrições vêm do GeoCatalogo (o texto que o site já usa para cada página);
    /// só os links externos têm a descrição escrita aqui. Mostradas por Views/Paginas/_Tema.cshtml (via PageController).
    /// Ao mudar um grupo do menu em Views/Shared/_Layout.cshtml, atualize também a lista abaixo.
    /// </summary>
    public static class HubTemas
    {
        public record Item(string Titulo, string Url, string? Descricao = null, bool Externo = false);
        public record Tema(string Slug, string Aba, string Titulo, string TituloSerif, string Intro, Item[] Itens, string? Pagina = null)
        {
            /// <summary>Endereço do tema: a página própria (Pagina) ou /slug.</summary>
            public string Link => Pagina ?? "/" + Slug;
        }

        public static readonly Dictionary<string, Tema> Temas = new Tema[]
        {
            new("vender-mais", "Soluções", "Vender", "mais.",
                "Soluções da CDL Uberlândia para divulgar sua empresa, conhecer melhor seus clientes e atrair novos compradores.",
                new Item[] {
                    new("CDL Mídia", "/cdl-midia"),
                    new("SPC Dados", "/spc-dados"),
                    new("Rede de Descontos", "/loja/descontos", Externo: true),
                }),
            new("gestao-e-economia", "Soluções", "Gestão", "e economia.",
                "Serviços para organizar a rotina da empresa, cuidar da equipe e acessar crédito e condições especiais.",
                new Item[] {
                    new("Certificado Digital", "/certificado-digital"),
                    new("CDL Gestão e NF-e", "/cdl-gestao"),
                    new("CDL Saúde", "/cdl-saude"),
                    new("Parceria BDMG", "/bdmg"),
                    new("Combo de Soluções", "/combo-solucoes"),
                    new("Consultoria", "https://cdluberlandia.cdludi.org.br/mentoria-de-negocios-fdc4b17686d8495e6609", "Mentoria de negócios com a CDL Uberlândia.", true),
                }),
            new("pessoas", "Soluções", "Pessoas", "para sua empresa.",
                "Programas da CDL Uberlândia para encontrar e formar profissionais: recrutamento, estágio e jovem aprendiz.",
                new Item[] {
                    new("Recrutamento e Seleção", "/cdl-talentos"),
                    new("CDL Estágio", "/cdl-estagio"),
                    new("CDL Aprendiz", "/cdl-aprendiz"),
                }),
            new("spc-para-empresas", "SPC", "SPC", "para sua empresa.",
                "Serviços do SPC Brasil oferecidos pela CDL Uberlândia para vender a prazo com mais segurança.",
                new Item[] {
                    new("SPC Consultas e Registro", "/spc-consulta-registro"),
                    new("SPC Avisa", "/spc-avisa"),
                    new("SPC Dados", "/spc-dados"),
                    new("Cadastro Positivo", "/cadastro-positivo"),
                }),
            new("spc-para-voce", "SPC", "SPC", "para você.",
                "Serviços do SPC Brasil para o consumidor: consultar o próprio CPF, negociar débitos e se proteger de fraudes com documentos.",
                new Item[] {
                    new("SPC Consulta CPF", "/spc-consulta"),
                    new("SPC Negociação de Débito", "/spc-negociacao"),
                    new("Alerta de Documentos", "/alerta-documento"),
                }),
            new("aprender", "Cursos e Eventos", "Aprender", "com a CDL.",
                "Cursos presenciais e a distância, eventos e treinamentos feitos sob medida para a sua equipe.",
                new Item[] {
                    new("Eventos", "/loja/eventos", Externo: true),
                    new("Cursos", "/loja/cursos", Externo: true),
                    new("Cursos EAD", "/loja/cursos-ead", Externo: true),
                    new("Treinamento in company", "/treinamento"),
                }),
            new("economizar", "Cursos e Eventos", "Economizar", "com a CDL.",
                "Descontos de parceiros, empresas parceiras e a plataforma CDL MenorPreço para fazer orçamentos com fornecedores associados.",
                new Item[] {
                    new("Descontos", "/loja/descontos", Externo: true),
                    new("Parceiros", "/loja/parceiros", Externo: true),
                    new("CDL MenorPreço", "/loja/menor-preco", Externo: true),
                }),
            new("espacos-para-eventos", "Soluções", "Espaços", "para eventos.",
                "Auditório, foyer e salas da CDL Uberlândia para locação: palestras, convenções, cursos, reuniões e eventos.",
                new Item[] {
                    new("CDL Convenções e Eventos", "/cdl-convencoes-e-eventos"),
                    new("CDL Auditório", "/cdl-auditorio"),
                    new("CDL Foyer", "/cdl-foyer"),
                    new("Sala Espaço Mais", "/sala-espaco-mais"),
                    new("Sala Executiva", "/sala-executiva"),
                    new("Sala Cecília Meireles", "/sala-cecilia-meireles"),
                    new("Sala Machado de Assis", "/sala-machado-de-assis"),
                }, Pagina: "/cdl-convencoes-e-eventos"),
            new("a-cdl", "A CDL", "A", "CDL Uberlândia.",
                "Quem somos, nossa história, a diretoria, a política de sustentabilidade e outras informações institucionais.",
                new Item[] {
                    new("Quem somos", "/quem-somos"),
                    new("Nossa História", "/nossa-historia"),
                    new("Reconhecimento", "/reconhecimento"),
                    new("Corporativo", "/corporativo"),
                    new("Diretoria", "/diretoria", "Composição da diretoria da CDL Uberlândia na gestão 2025/2026."),
                    new("Política de Sustentabilidade", "/politica-de-sustentabilidade"),
                    new("Selo Empresa Cidadã", "https://empresacidada.cdludi.org.br/", "Site do Selo Empresa Cidadã da CDL Uberlândia.", true),
                    new("Licitações", "/licitacoes"),
                }),
            new("fundacao", "A Fundação", "Serviços", "da Fundação CDL.",
                "Programas da Fundação CDL para formar jovens, encontrar talentos e capacitar equipes, com apoio em todas as etapas.",
                new Item[] {
                    new("Jovem Aprendiz", "/cdl-aprendiz", "Fique em dia com sua cota de jovem aprendiz."),
                    new("CDL Estágio", "/cdl-estagio"),
                    new("CDL Talentos", "/cdl-talentos", "Encontre o talento que a sua empresa precisa."),
                    new("Treinamento In Company", "/treinamento", "Prepare a sua equipe!"),
                    new("Consultorias em Gestão e Inovação", "https://cdluberlandia.cdludi.org.br/mentoria-de-negocios", "Mentoria de negócios para impulsionar o crescimento da sua empresa.", true),
                    new("Vagas de emprego e estágio", "https://fundacaocdl.abler.com.br/", "Confira as vagas de emprego e estágio disponíveis e cadastre seu currículo.", true),
                }, Pagina: "/fundacao-cdl#servicos"),
        }.ToDictionary(t => t.Slug);

        /// <summary>Texto de apresentação copiado da própria página do serviço (ResumosServicos), se houver.</summary>
        public static string Explicacao(Item i) => ResumosServicos.Textos.TryGetValue(i.Url, out var t) ? t : "";

        /// <summary>Cor de destaque de cada tema (painéis e ícones).</summary>
        public static string Cor(string slug) => slug switch
        {
            "espacos-para-eventos" => "#7a4dff",
            "vender-mais" => "#f2711c",
            "gestao-e-economia" => "#0b5cd5",
            "pessoas" => "#119a5a",
            "spc-para-empresas" => "#0b5cd5",
            "spc-para-voce" => "#0e8f9b",
            "aprender" => "#7a4dff",
            "economizar" => "#119a5a",
            "fundacao" => "#0e8f9b",
            _ => "#0a2f7a",
        };

        /// <summary>Ícone (traço SVG 24x24) de cada serviço, pelo endereço.</summary>
        public static string Icone(string url) => url switch
        {
            "/cdl-convencoes-e-eventos" => "<path d='M3 21h18M5 21V8l7-5 7 5v13'/><path d='M9 21v-6h6v6'/>",
            "/cdl-auditorio" => "<path d='M4 20h16M6 20v-4M18 20v-4M8 16h8'/><rect x='5' y='4' width='14' height='8' rx='1'/>",
            "/cdl-foyer" => "<path d='M8 22h8M12 15v7M7 3h10l-1 7a4 4 0 0 1-8 0z'/>",
            "/sala-espaco-mais" or "/sala-executiva" or "/sala-cecilia-meireles" or "/sala-machado-de-assis" => "<rect x='3' y='7' width='18' height='10' rx='2'/><path d='M7 17v3M17 17v3M7 7V4h10v3'/>",
            "/cdl-midia" => "<path d='M3 11v2a1 1 0 0 0 1 1h2l5 4V6L6 10H4a1 1 0 0 0-1 1z'/><path d='M15 9a3 3 0 0 1 0 6M18 6a7 7 0 0 1 0 12'/>",
            "/spc-dados" => "<ellipse cx='12' cy='5' rx='8' ry='3'/><path d='M4 5v14c0 1.7 3.6 3 8 3s8-1.3 8-3V5M4 12c0 1.7 3.6 3 8 3s8-1.3 8-3'/>",
            "/loja/descontos" => "<path d='M20 12 12 20l-9-9V4h7z'/><circle cx='7.5' cy='7.5' r='1.5'/>",
            "/certificado-digital" => "<rect x='3' y='4' width='18' height='12' rx='2'/><circle cx='12' cy='10' r='2.5'/><path d='m10 15-1 6 3-2 3 2-1-6'/>",
            "/cdl-gestao" => "<path d='M6 2h9l4 4v16l-3-2-3 2-3-2-3 2-1-1z'/><path d='M9 9h6M9 13h6'/>",
            "/cdl-saude" => "<path d='M20.8 4.6a5.5 5.5 0 0 0-7.8 0L12 5.7l-1-1.1a5.5 5.5 0 0 0-7.8 7.8L12 21l8.8-8.6a5.5 5.5 0 0 0 0-7.8z'/>",
            "/bdmg" => "<path d='M3 21h18M4 10h16M12 3l9 5H3zM6 10v8M10 10v8M14 10v8M18 10v8'/>",
            "/combo-solucoes" => "<path d='M21 8 12 3 3 8l9 5zM3 8v8l9 5 9-5V8M12 13v8'/>",
            "/cdl-talentos" => "<circle cx='9' cy='8' r='4'/><path d='M2 21c0-4 3-6 7-6s7 2 7 6M17 11l2 2 4-4'/>",
            "/cdl-estagio" => "<path d='M22 9 12 4 2 9l10 5zM6 11v5c3 2 9 2 12 0v-5'/>",
            "/cdl-aprendiz" => "<path d='M12 22V12M12 12C12 7 8 5 4 5c0 5 3 7 8 7zM12 14c0-4 3-6 8-6 0 4-3 6-8 6z'/>",
            "/spc-consulta-registro" => "<circle cx='11' cy='11' r='7'/><path d='m20 20-3.5-3.5M8 11h6M11 8v6'/>",
            "/spc-avisa" => "<path d='M6 8a6 6 0 0 1 12 0c0 7 3 9 3 9H3s3-2 3-9M10 21a2 2 0 0 0 4 0'/>",
            "/cadastro-positivo" => "<circle cx='12' cy='12' r='9'/><path d='m8 12 3 3 5-6'/>",
            "/spc-consulta" => "<rect x='3' y='5' width='18' height='14' rx='2'/><circle cx='9' cy='11' r='2'/><path d='M6 16c.5-1.5 1.7-2 3-2s2.5.5 3 2M14 10h4M14 13h3'/>",
            "/spc-negociacao" => "<path d='M8 13 3 8l4-4 5 5M16 11l5 5-4 4-5-5M7 12l5 5M12 7l5 5'/>",
            "/alerta-documento" => "<path d='M12 3 2 21h20zM12 10v5M12 18h.01'/>",
            "/loja/eventos" => "<rect x='3' y='5' width='18' height='16' rx='2'/><path d='M3 10h18M8 3v4M16 3v4'/>",
            "/loja/cursos" => "<path d='M4 4h6a3 3 0 0 1 3 3v13a2 2 0 0 0-2-2H4zM20 4h-6a3 3 0 0 0-3 3'/><path d='M20 4v14h-7'/>",
            "/loja/cursos-ead" => "<rect x='3' y='4' width='18' height='12' rx='2'/><path d='M8 20h8M12 16v4M10 8l4 2-4 2z'/>",
            "/treinamento" => "<path d='M3 4h18M5 4v10h14V4M12 14v4M8 21l4-3 4 3'/>",
            "/loja/parceiros" => "<path d='M3 9 5 4h14l2 5M3 9v11h18V9M3 9h18M9 20v-6h6v6'/>",
            "/loja/menor-preco" => "<circle cx='9' cy='9' r='6'/><path d='M15.5 9.5A6 6 0 1 1 9.5 15.5'/>",
            "/quem-somos" => "<circle cx='12' cy='12' r='9'/><path d='M12 11v6M12 7h.01'/>",
            "/nossa-historia" => "<circle cx='12' cy='12' r='9'/><path d='M12 7v5l3 2'/>",
            "/fundacao-cdl" => "<path d='M11 14h2a2 2 0 0 0 0-4h-3c-.6 0-1.1.2-1.4.6L3 16M7 20l1.6-1.4c.3-.4.8-.6 1.4-.6h4c1.1 0 2.1-.4 2.8-1.2l4.6-4.4a2 2 0 0 0-2.8-2.8L14 13'/>",
            "/reconhecimento" => "<circle cx='12' cy='9' r='6'/><path d='m9 14-2 8 5-3 5 3-2-8'/>",
            "/corporativo" => "<circle cx='12' cy='5' r='2.5'/><circle cx='5' cy='19' r='2.5'/><circle cx='19' cy='19' r='2.5'/><path d='M12 7.5v4M12 11.5 6.5 17M12 11.5l5.5 5.5'/>",
            "/diretoria" => "<circle cx='9' cy='8' r='3.5'/><circle cx='17' cy='9' r='2.5'/><path d='M2 20c0-3.5 3-5.5 7-5.5s7 2 7 5.5M16 14.5c3 0 6 1.5 6 4.5'/>",
            "/politica-de-sustentabilidade" => "<path d='M11 20A7 7 0 0 1 4 13C4 7 9 4 20 4c0 11-3 16-9 16zM4 21c3-6 7-9 11-11'/>",
            "https://fundacaocdl.abler.com.br/" => "<rect x='3' y='7' width='18' height='13' rx='2'/><path d='M8 7V5a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2M3 13h18'/>",
            "https://cdluberlandia.cdludi.org.br/mentoria-de-negocios" => "<path d='M9 18h6M10 22h4M12 2a7 7 0 0 0-4 12.7V17h8v-2.3A7 7 0 0 0 12 2z'/>",
            "/licitacoes" => "<path d='M6 2h9l5 5v15H6z'/><path d='M14 2v6h6M9 13h8M9 17h6'/>",
            _ => url.StartsWith("http") ? "<path d='M14 4h6v6M20 4l-9 9M18 14v6H4V6h6'/>" : "<path d='M5 12h14M13 6l6 6-6 6'/>",
        };

        /// <summary>Descrição do item: a do próprio item ou, se não houver, a do GeoCatalogo para o mesmo endereço.</summary>
        public static string Descricao(Item i) =>
            i.Descricao ?? GeoCatalogo.Grupos.SelectMany(g => g.Paginas).FirstOrDefault(p => p.Caminho == i.Url)?.Descricao ?? "";
    }
}
