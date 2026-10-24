// Worker da página de divulgação do PKHeX Modern em https://allgenwiki.com/pkhex/.
// Mesmo esquema da página /sobre/ da wiki (repositório allgenwiki-site):
//   /pkhex        301 para /pkhex/ (com a barra, caminhos relativos resolvem dentro da pasta)
//   /pkhex/...    sai do binding ASSETS (a pasta docs/), sem o prefixo
//   /pkhexxyz     não é desta página: segue para a wiki
// Tudo o que este Worker responde leva os cabeçalhos de segurança da wiki.
const DOMINIO = 'allgenwiki.com';
const PREFIXO = '/pkhex';

// Os mesmos cabeçalhos de segurança da wiki (_headers do HoennKantoWiki).
const SEGURANCA = {
    'X-Content-Type-Options': 'nosniff',
    'Referrer-Policy': 'strict-origin-when-cross-origin',
    'X-Frame-Options': 'SAMEORIGIN',
    'Content-Security-Policy': "frame-ancestors 'self'",
    'Permissions-Policy': 'camera=(), microphone=(), geolocation=(), payment=(), usb=()',
};

function comSeguranca(resposta) {
    // Respostas de redirect e de fetch têm cabeçalhos imutáveis: copia antes.
    const nova = new Response(resposta.body, resposta);
    for (const [nome, valor] of Object.entries(SEGURANCA)) nova.headers.set(nome, valor);
    return nova;
}

export default {
    async fetch(request, env) {
        const url = new URL(request.url);
        if (url.pathname !== PREFIXO && !url.pathname.startsWith(`${PREFIXO}/`)) {
            return fetch(request);
        }
        if (url.pathname === PREFIXO) {
            return comSeguranca(Response.redirect(`https://${DOMINIO}${PREFIXO}/${url.search}`, 301));
        }
        url.pathname = url.pathname.slice(PREFIXO.length);
        return comSeguranca(await env.ASSETS.fetch(new Request(url, request)));
    },
};
