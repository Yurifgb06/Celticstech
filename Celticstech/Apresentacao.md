# Pitch do Projeto

Olá, somos o grupo responsável pelo projeto Celticstech, desenvolvido para a Global Solution 2026 da disciplina Advanced Business Development with .NET.

O Celticstech é uma plataforma inteligente de monitoramento agrícola que aplica conceitos de Economia Espacial para auxiliar associações agrícolas e produtores rurais do Nordeste brasileiro na tomada de decisões preventivas.

O principal problema que buscamos resolver é a dificuldade de acompanhar e interpretar rapidamente informações climáticas que impactam diretamente a produção agrícola. Mudanças na temperatura, períodos de seca, baixa umidade do ar e ventos intensos podem causar prejuízos significativos quando não são identificados com antecedência.

Pensando nisso, desenvolvemos uma solução capaz de integrar informações cadastrais, localização geográfica e dados climáticos em tempo real para gerar diagnósticos automáticos e recomendações técnicas para o setor agrícola.

A aplicação da Economia Espacial acontece por meio da utilização das coordenadas geográficas das regiões monitoradas. Cada região cadastrada possui latitude e longitude associadas, permitindo que o sistema consulte dados climáticos específicos daquela localidade. Dessa forma, as análises realizadas não são genéricas, mas sim adaptadas às características e condições climáticas reais de cada região.

O sistema permite o cadastro de regiões, associações agrícolas, cultivos e agricultores. Essas informações são organizadas e relacionadas entre si para representar de forma mais fiel o cenário agrícola monitorado pela plataforma.

A partir dessas informações, o Celticstech realiza consultas em tempo real na Open-Meteo API, obtendo indicadores como temperatura, umidade relativa do ar, volume de chuva e velocidade do vento.

Com base nesses dados, o sistema calcula automaticamente um Score de Risco que varia de 0 a 100. Esse score representa o nível de risco climático para determinado cultivo em uma determinada região.

Os resultados são classificados em três níveis:

Baixo risco, para scores entre 0 e 39;
Risco moderado, para scores entre 40 e 69;
Alto risco, para scores entre 70 e 100.

Além da classificação do risco, o sistema gera automaticamente uma recomendação técnica completa contendo resumo do risco identificado, justificativa da análise, ações recomendadas, prioridade de atendimento, prazo sugerido para execução e observações técnicas baseadas nos dados climáticos coletados.

Outro diferencial da solução é o armazenamento do histórico climático. Todos os diagnósticos gerados ficam registrados, permitindo consultas futuras e acompanhamento da evolução das condições ambientais ao longo do tempo.

Para o desenvolvimento da solução utilizamos .NET 8, ASP.NET Core Web API, Entity Framework Core, PostgreSQL, Swagger, Docker e integração com a Open-Meteo API. Também desenvolvemos um frontend responsivo utilizando HTML, CSS, JavaScript, Bootstrap e Chart.js para facilitar a visualização das informações pelos usuários.

Entre os principais benefícios do Celticstech estão a automatização da análise climática, a redução dos riscos associados à produção agrícola, o apoio à tomada de decisões preventivas e a utilização prática dos conceitos de Economia Espacial aplicados ao agronegócio.

Concluímos que o Celticstech demonstra como a tecnologia pode ser utilizada para transformar dados climáticos em informações úteis para produtores rurais e associações agrícolas, contribuindo para uma gestão mais eficiente, preventiva e baseada em dados reais.