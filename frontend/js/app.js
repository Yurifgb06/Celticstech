const API_CANDIDATES = [
    window.location.origin,
    "http://localhost:5000",
    "https://localhost:5001",
    "http://localhost:8080",
    "https://localhost:7113",
    "https://localhost:7143",
    "http://localhost:5143"
];

let API_BASE = "";

const HISTORY_PAGE_SIZE = 6;
const THEME_STORAGE_KEY = "celticstech-theme";
const REQUEST_TIMEOUT = 3500;

const entityConfig = {
    regions: {
        endpoint: "/api/Regioes",
        title: "Regioes",
        singular: "regiao",
        idField: "idRegiao",
        kicker: "Cadastro geografico",
        description: "Consulte e mantenha as regioes monitoradas pela plataforma."
    },
    associations: {
        endpoint: "/api/Associacoes",
        title: "Associacoes",
        singular: "associacao",
        idField: "idAssociacao",
        kicker: "Entidades parceiras",
        description: "Gerencie as associacoes vinculadas as regioes atendidas."
    },
    farmers: {
        title: "Agricultores",
        singular: "agricultor",
        available: false,
        kicker: "Produtores rurais",
        description: "Este cadastro ainda nao possui endpoint no backend."
    },
    crops: {
        endpoint: "/api/Cultivos",
        title: "Cultivos",
        singular: "cultivo",
        idField: "idCultivo",
        kicker: "Producao agricola",
        description: "Mantenha as culturas usadas nas recomendacoes da plataforma."
    },
    recommendations: {
        endpoint: "/api/Recomendacoes",
        title: "Recomendacoes",
        singular: "recomendacao",
        idField: "idRecomendacao",
        kicker: "Orientacoes agricolas",
        description: "Consulte e mantenha os vinculos que geram orientacoes agricolas."
    }
};

const state = {
    regions: [],
    associations: [],
    crops: [],
    recommendations: [],
    selectedRegionId: null,
    history: [],
    historyPage: 1,
    chart: null,
    validation: {},
    managementEntity: "regions",
    editingId: null,
    deleteTarget: null,
    entityModal: null,
    deleteModal: null,
    historyDetailModal: null
};

const elements = {};

document.addEventListener("DOMContentLoaded", initializeApp);

async function initializeApp() {
    cacheElements();
    aplicarTemaSalvo();
    bindEvents();
    initializeChart();
    initializeModals();

    try {
        await detectarApi();
        await Promise.all([
            verificarSistema(),
            loadDashboard(),
            loadRegions(),
            loadHistory(),
            loadReferenceData()
        ]);
        await loadManagementData("regions");
    } catch (error) {
        console.error("Falha ao iniciar o front-end:", error);
        setIntegrationStatus(false);
        setValidationSummary("error");
        showGlobalError(
            "Nao foi possivel conectar a API. Inicie o backend com dotnet run e confira a porta exibida no terminal."
        );
        showToast("Nao foi possivel conectar a API.", "error");
    }
}

function cacheElements() {
    [
        "globalAlert", "openMeteoStatus", "lastUpdate", "refreshDashboard",
        "refreshHistory", "regionSelect", "consultClimate", "generateDiagnosis",
        "diagnosisResult", "historyTableBody", "historyPagination", "historyCount",
        "themeToggle", "verifySystem", "apiBaseCurrent", "validationSummaryIcon",
        "validationSummaryTitle", "validationSummaryText", "managementTabs",
        "managementKicker", "managementEntityTitle", "managementDescription",
        "managementTableWrapper", "managementTableHead", "managementTableBody",
        "managementUnavailable", "newRecord", "refreshManagement", "entityForm",
        "entityFormFields", "entityModalTitle", "entityModalKicker", "saveEntity",
        "deleteRecordName", "confirmDelete", "toastContainer", "historyDetailBody"
    ].forEach((id) => {
        elements[id] = document.getElementById(id);
    });
}

function bindEvents() {
    elements.refreshDashboard.addEventListener("click", refreshAllData);
    elements.refreshHistory.addEventListener("click", refreshHistoryWithFeedback);
    elements.historyTableBody.addEventListener("click", handleHistoryInteraction);
    elements.historyTableBody.addEventListener("keydown", handleHistoryInteraction);
    elements.regionSelect.addEventListener("change", handleRegionChange);
    elements.consultClimate.addEventListener("click", consultSelectedClimate);
    elements.generateDiagnosis.addEventListener("click", generateDiagnosis);
    elements.themeToggle.addEventListener("click", alternarTema);
    elements.verifySystem.addEventListener("click", verifySystemWithFeedback);
    elements.managementTabs.addEventListener("click", handleManagementTab);
    elements.managementTableBody.addEventListener("click", handleManagementAction);
    elements.newRecord.addEventListener("click", () => openEntityForm());
    elements.refreshManagement.addEventListener("click", refreshManagementWithFeedback);
    elements.entityForm.addEventListener("submit", saveEntity);
    elements.confirmDelete.addEventListener("click", deleteEntity);
}

function initializeModals() {
    state.entityModal = createModalController("entityModal");
    state.deleteModal = createModalController("deleteModal");
    state.historyDetailModal = createModalController("historyDetailModal");

    if (typeof bootstrap === "undefined") {
        document.addEventListener("click", (event) => {
            const dismissButton = event.target.closest('[data-bs-dismiss="modal"]');

            if (dismissButton) {
                createModalController(dismissButton.closest(".modal")?.id).hide();
            }
        });
    }
}

function createModalController(id) {
    const modal = document.getElementById(id);

    if (!modal) {
        return { show() {}, hide() {} };
    }

    if (typeof bootstrap !== "undefined") {
        return bootstrap.Modal.getOrCreateInstance(modal);
    }

    return {
        show() {
            modal.style.display = "block";
            modal.classList.add("show");
            modal.removeAttribute("aria-hidden");
            modal.setAttribute("aria-modal", "true");
            modal.setAttribute("role", "dialog");
            document.body.classList.add("modal-open");
            getFallbackBackdrop().classList.add("show");
        },
        hide() {
            modal.style.display = "none";
            modal.classList.remove("show");
            modal.setAttribute("aria-hidden", "true");
            modal.removeAttribute("aria-modal");
            modal.removeAttribute("role");
            document.body.classList.remove("modal-open");
            document.getElementById("fallbackModalBackdrop")?.remove();
        }
    };
}

function getFallbackBackdrop() {
    let backdrop = document.getElementById("fallbackModalBackdrop");

    if (!backdrop) {
        backdrop = document.createElement("div");
        backdrop.id = "fallbackModalBackdrop";
        backdrop.className = "modal-backdrop fade";
        document.body.appendChild(backdrop);
    }

    return backdrop;
}

function aplicarTemaSalvo() {
    let savedTheme = null;

    try {
        savedTheme = localStorage.getItem(THEME_STORAGE_KEY);
    } catch {
        savedTheme = null;
    }

    const preferredTheme = window.matchMedia("(prefers-color-scheme: dark)").matches
        ? "dark"
        : "light";
    const theme = ["dark", "light"].includes(savedTheme) ? savedTheme : preferredTheme;

    document.documentElement.dataset.theme = theme;
    atualizarBotaoTema(theme);
}

function alternarTema() {
    const currentTheme = document.documentElement.dataset.theme || "light";
    const nextTheme = currentTheme === "dark" ? "light" : "dark";
    document.documentElement.dataset.theme = nextTheme;

    try {
        localStorage.setItem(THEME_STORAGE_KEY, nextTheme);
    } catch {
        // A preferencia permanece ativa durante a sessao.
    }

    atualizarBotaoTema(nextTheme);
    updateChartTheme();
}

function atualizarBotaoTema(theme) {
    const isDark = theme === "dark";
    elements.themeToggle.innerHTML = isDark
        ? '<i class="bi bi-sun-fill"></i><span>Modo claro</span>'
        : '<i class="bi bi-moon-stars-fill"></i><span>Modo escuro</span>';
    elements.themeToggle.setAttribute(
        "aria-label",
        isDark ? "Ativar modo claro" : "Ativar modo escuro"
    );
}

async function detectarApi(force = false) {
    if (API_BASE && !force) {
        return API_BASE;
    }

    API_BASE = "";
    elements.apiBaseCurrent.textContent = "Detectando...";

    const candidates = [...new Set(API_CANDIDATES.filter((candidate) =>
        candidate && candidate !== "null" && /^https?:\/\//i.test(candidate)
    ))];

    for (const base of candidates) {
        const controller = new AbortController();
        const timeout = window.setTimeout(() => controller.abort(), REQUEST_TIMEOUT);

        try {
            const response = await fetch(`${base}/health`, {
                cache: "no-store",
                signal: controller.signal
            });

            if (response.ok) {
                API_BASE = base.replace(/\/$/, "");
                elements.apiBaseCurrent.textContent = API_BASE;
                return API_BASE;
            }
        } catch (error) {
            console.debug(`API nao encontrada em ${base}:`, error.message);
        } finally {
            window.clearTimeout(timeout);
        }
    }

    elements.apiBaseCurrent.textContent = "Nenhuma API detectada";
    throw new Error("Nenhuma API disponivel");
}

async function verificarSistema(redetectar = false) {
    setButtonLoading(elements.verifySystem, true, "Verificando...");
    setValidationSummary("pending");

    [
        "validationBackend", "validationHealth", "validationDashboard",
        "validationRegions", "validationHistory", "validationApiBase"
    ].forEach((id) => atualizarStatusValidacao(id, "pending"));

    if (redetectar || !API_BASE) {
        try {
            await detectarApi(true);
        } catch (error) {
            console.error("Nenhuma API respondeu a validacao:", error);
            state.validation = {
                backend: false, health: false, dashboard: false,
                regions: false, history: false, apiBase: false
            };
            atualizarValidacaoIndisponivel();
            setValidationSummary("error");
            setIntegrationStatus(false);
            setButtonLoading(elements.verifySystem, false);
            return false;
        }
    }

    const tests = await Promise.all([
        testSystemEndpoint("/health", "text"),
        testSystemEndpoint("/api/Dashboard/resumo", "object"),
        testSystemEndpoint("/api/Regioes", "array"),
        testSystemEndpoint("/api/Recomendacoes/historico-climatico", "array")
    ]);
    const [health, dashboard, regions, history] = tests;
    const backendConnected = tests.some((test) => test.ok);

    state.validation = {
        backend: backendConnected,
        health: health.ok,
        dashboard: dashboard.ok,
        regions: regions.ok,
        history: history.ok,
        apiBase: Boolean(API_BASE)
    };

    atualizarStatusValidacao(
        "validationBackend",
        backendConnected ? "success" : "error",
        backendConnected ? "Backend conectado" : "Backend indisponivel",
        backendConnected ? "A API respondeu aos testes de conexao." : "Nenhuma resposta foi recebida."
    );
    atualizarStatusValidacao(
        "validationHealth",
        health.ok ? "success" : "error",
        health.ok ? "Health Check OK" : "Health Check indisponivel",
        health.ok ? "O endpoint /health retornou 200." : health.message
    );
    atualizarStatusValidacao(
        "validationDashboard",
        dashboard.ok ? "success" : "error",
        dashboard.ok ? "Dashboard carregado" : "Dashboard nao carregado",
        dashboard.ok ? "O resumo operacional retornou dados." : dashboard.message
    );
    atualizarStatusValidacao(
        "validationRegions",
        regions.ok ? "success" : "error",
        regions.ok ? "Regioes carregadas" : "Regioes nao carregadas",
        regions.ok ? "A API retornou uma lista de regioes." : regions.message
    );
    atualizarStatusValidacao(
        "validationHistory",
        history.ok ? "success" : "error",
        history.ok ? "Historico climatico carregado" : "Historico indisponivel",
        history.ok ? "O endpoint de historico esta acessivel." : history.message
    );
    atualizarStatusValidacao(
        "validationApiBase",
        API_BASE ? "success" : "error",
        API_BASE ? "API_BASE detectado" : "API_BASE indisponivel",
        API_BASE || "Nenhuma URL respondeu ao Health Check."
    );

    const allValid = Object.values(state.validation).every(Boolean);
    setValidationSummary(allValid ? "success" : "error");
    setIntegrationStatus(backendConnected && health.ok);
    setButtonLoading(elements.verifySystem, false);
    return allValid;
}

async function verifySystemWithFeedback() {
    const valid = await verificarSistema(true);
    showToast(
        valid ? "Sistema validado com sucesso." : "A validacao encontrou endpoints indisponiveis.",
        valid ? "success" : "warning"
    );
}

function atualizarValidacaoIndisponivel() {
    [
        ["validationBackend", "Backend indisponivel", "Nenhuma URL candidata respondeu."],
        ["validationHealth", "Health Check indisponivel", "Nao foi possivel acessar /health."],
        ["validationDashboard", "Dashboard nao carregado", "A API nao foi detectada."],
        ["validationRegions", "Regioes nao carregadas", "A API nao foi detectada."],
        ["validationHistory", "Historico indisponivel", "A API nao foi detectada."],
        ["validationApiBase", "API_BASE indisponivel", "Nenhuma URL candidata respondeu."]
    ].forEach(([id, title, detail]) => atualizarStatusValidacao(id, "error", title, detail));
}

function atualizarStatusValidacao(
    elementId,
    status,
    title = "Verificando...",
    detail = "Aguarde a conclusao do teste."
) {
    const element = document.getElementById(elementId);

    if (!element) {
        return;
    }

    const icon = status === "success"
        ? "bi-check-lg"
        : status === "error"
            ? "bi-x-lg"
            : "bi-three-dots";

    element.classList.remove("is-pending", "is-success", "is-error");
    element.classList.add(`is-${status}`);
    element.querySelector(".validation-check-icon").innerHTML = `<i class="bi ${icon}"></i>`;
    element.querySelector("strong").textContent = title;
    element.querySelector("small").textContent = detail;
}

function setValidationSummary(status) {
    const settings = {
        pending: {
            icon: "bi-hourglass-split",
            title: "Verificando conexoes...",
            text: "Aguarde enquanto o front localiza e valida a API."
        },
        success: {
            icon: "bi-check-circle-fill",
            title: "Sistema validado com sucesso",
            text: "Backend e endpoints principais estao acessiveis."
        },
        error: {
            icon: "bi-exclamation-triangle-fill",
            title: "Nao foi possivel validar o sistema",
            text: "Inicie o backend com dotnet run e confira a porta exibida no terminal."
        }
    };
    const current = settings[status];

    elements.validationSummaryIcon.className = `validation-summary-icon is-${status}`;
    elements.validationSummaryIcon.innerHTML = `<i class="bi ${current.icon}"></i>`;
    elements.validationSummaryTitle.textContent = current.title;
    elements.validationSummaryText.textContent = current.text;
}

async function testSystemEndpoint(path, expectedType) {
    const controller = new AbortController();
    const timeout = window.setTimeout(() => controller.abort(), REQUEST_TIMEOUT);

    try {
        const response = await fetch(`${API_BASE}${path}`, {
            headers: { Accept: "application/json, text/plain" },
            signal: controller.signal
        });

        if (!response.ok) {
            return { ok: false, message: `${path} retornou HTTP ${response.status}.` };
        }

        if (expectedType === "text") {
            await response.text();
            return { ok: true };
        }

        const data = await response.json();
        const validData = expectedType === "array"
            ? Array.isArray(data)
            : data !== null && typeof data === "object" && !Array.isArray(data);
        return {
            ok: validData,
            message: validData ? "Endpoint acessivel." : `${path} retornou um formato inesperado.`
        };
    } catch (error) {
        console.error(`Erro ao validar ${path}:`, error);
        return {
            ok: false,
            message: error.name === "AbortError"
                ? `${path} excedeu o tempo limite.`
                : "Verifique se a API esta rodando e se API_BASE usa a porta correta."
        };
    } finally {
        window.clearTimeout(timeout);
    }
}

async function apiFetch(path, options = {}) {
    if (!API_BASE) {
        await detectarApi();
    }

    try {
        const response = await fetch(`${API_BASE}${path}`, {
            ...options,
            headers: {
                Accept: "application/json",
                ...(options.body ? { "Content-Type": "application/json" } : {}),
                ...options.headers
            }
        });

        if (!response.ok) {
            const message = await readErrorMessage(response);
            throw new Error(message || `Falha na requisicao (${response.status}).`);
        }

        if (response.status === 204) {
            return null;
        }

        return response.json();
    } catch (error) {
        console.error(`${options.method || "GET"} ${path}:`, error);
        throw error;
    }
}

async function readErrorMessage(response) {
    try {
        const contentType = response.headers.get("content-type") || "";

        if (contentType.includes("application/json")) {
            const body = await response.json();

            if (body.errors) {
                return Object.values(body.errors).flat().join(" ");
            }

            return body.title || body.message || body.detail || JSON.stringify(body);
        }

        return await response.text();
    } catch {
        return "";
    }
}

async function refreshAllData() {
    setButtonLoading(elements.refreshDashboard, true, "Atualizando...");

    try {
        const results = await Promise.all([
            loadDashboard(),
            loadRegions(),
            loadHistory(),
            verificarSistema(),
            loadReferenceData()
        ]);
        await loadManagementData(state.managementEntity);

        if (results.every((result) => result !== false)) {
            showToast("Painel atualizado com sucesso.", "success");
        } else {
            showToast("O painel foi atualizado parcialmente.", "warning");
        }
    } catch (error) {
        console.error("Erro ao atualizar o painel:", error);
        showToast("Nao foi possivel atualizar o painel.", "error");
    } finally {
        setButtonLoading(elements.refreshDashboard, false);
    }
}

async function loadDashboard() {
    try {
        const data = await apiFetch("/api/Dashboard/resumo");
        setText("totalRegioes", data.totalRegioes);
        setText("totalAssociacoes", data.totalAssociacoes);
        setText("totalCultivos", data.totalCultivos);
        setText("totalRecomendacoes", data.totalRecomendacoes);
        setText("statusSistema", data.statusSistema || "Operacional");
        setText("integracaoOpenMeteo", `Open-Meteo ${data.integracaoOpenMeteo || "Ativa"}`);
        setIntegrationStatus(true);
        updateTimestamp();
        return true;
    } catch (error) {
        setIntegrationStatus(false);
        console.error("Nao foi possivel carregar o dashboard:", error);
        return false;
    }
}

async function loadRegions() {
    try {
        const regions = await apiFetch("/api/Regioes");
        state.regions = Array.isArray(regions) ? regions : [];
        populateRegionSelect();

        if (state.regions.length > 0) {
            const selectedStillExists = state.regions.some(
                (region) => String(region.idRegiao) === String(state.selectedRegionId)
            );
            state.selectedRegionId = selectedStillExists
                ? String(state.selectedRegionId)
                : String(state.regions[0].idRegiao);
            elements.regionSelect.value = state.selectedRegionId;
            elements.generateDiagnosis.disabled = false;
            elements.consultClimate.disabled = false;
            await loadClimate(state.selectedRegionId, false);
        } else {
            state.selectedRegionId = null;
            elements.regionSelect.innerHTML = '<option value="">Nenhuma regiao cadastrada</option>';
            elements.regionSelect.disabled = true;
            elements.consultClimate.disabled = true;
            elements.generateDiagnosis.disabled = true;
        }

        return true;
    } catch (error) {
        console.error("Nao foi possivel carregar as regioes:", error);
        elements.regionSelect.innerHTML = '<option value="">Erro ao carregar regioes</option>';
        elements.consultClimate.disabled = true;
        return false;
    }
}

function populateRegionSelect() {
    elements.regionSelect.innerHTML = state.regions.map((region) =>
        `<option value="${region.idRegiao}">${escapeHtml(region.nomeRegiao)} - ${escapeHtml(region.ufRegiao)}</option>`
    ).join("");
    elements.regionSelect.disabled = state.regions.length === 0;
}

async function handleRegionChange(event) {
    state.selectedRegionId = event.target.value;
    elements.diagnosisResult.classList.add("d-none");
    elements.generateDiagnosis.disabled = !state.selectedRegionId;
    elements.consultClimate.disabled = !state.selectedRegionId;
}

async function consultSelectedClimate() {
    if (!state.selectedRegionId) {
        showToast("Selecione uma regiao para consultar o clima.", "warning");
        return;
    }

    const success = await loadClimate(state.selectedRegionId, true);

    if (success) {
        showToast("Dados climaticos atualizados.", "success");
    }
}

async function loadClimate(regionId, notifyOnError = true) {
    setClimateLoading(true);

    try {
        const data = await apiFetch(`/api/Satelite/clima/regiao/${regionId}`);
        renderClimate(data);
        setIntegrationStatus(true);
        updateTimestamp();
        return true;
    } catch (error) {
        clearClimate();
        setIntegrationStatus(false);
        console.error("Nao foi possivel consultar o clima:", error);

        if (notifyOnError) {
            showToast(`Erro ao consultar o clima: ${error.message}`, "error");
        }

        return false;
    } finally {
        setClimateLoading(false);
    }
}

function renderClimate(data) {
    setText("climateRegion", data.regiao || "Regiao");
    setText("climateUf", data.uf || "--");
    setText("temperatureValue", formatNumber(data.temperatura, 1));
    setText("humidityValue", formatNumber(data.umidade, 0));
    setText("rainValue", formatNumber(data.chuva, 1));
    setText("windValue", formatNumber(data.velocidadeVento, 1));
    setText("riskScore", data.scoreRisco ?? 0);
    setText("riskLabel", getRiskDescription(data.nivelRisco));
    document.getElementById("riskGauge").style.setProperty(
        "--risk-value",
        clamp(data.scoreRisco ?? 0, 0, 100)
    );
    applyRiskBadge(document.getElementById("riskBadge"), data.nivelRisco);
    updateChart(data);
}

function clearClimate() {
    ["temperatureValue", "humidityValue", "rainValue", "windValue", "riskScore"]
        .forEach((id) => setText(id, "--"));
    setText("climateRegion", "Dados indisponiveis");
    setText("climateUf", "--");
    setText("riskLabel", "Nao foi possivel calcular");
    document.getElementById("riskGauge").style.setProperty("--risk-value", 0);
    applyRiskBadge(document.getElementById("riskBadge"), "");
    updateChart({ temperatura: 0, umidade: 0, chuva: 0, velocidadeVento: 0 });
}

async function generateDiagnosis() {
    if (!state.selectedRegionId) {
        showToast("Selecione uma regiao antes de gerar o diagnostico.", "warning");
        return;
    }

    setDiagnosisLoading(true);

    try {
        const data = await apiFetch(`/api/Diagnostico/regiao/${state.selectedRegionId}`);
        renderDetailedDiagnosis(data);
        elements.diagnosisResult.classList.remove("d-none");
        elements.diagnosisResult.scrollIntoView({ behavior: "smooth", block: "nearest" });
        await Promise.all([loadHistory(), loadDashboard(), loadReferenceData()]);

        if (state.managementEntity === "recommendations") {
            await loadManagementData("recommendations");
        }

        showToast("Diagnostico gerado com sucesso.", "success");
    } catch (error) {
        console.error("Nao foi possivel gerar o diagnostico:", error);
        showToast(`Erro ao gerar diagnostico: ${error.message}`, "error");
    } finally {
        setDiagnosisLoading(false);
    }
}

function renderDetailedDiagnosis(data) {
    const details = normalizeDiagnosis(data);
    setText("diagnosisScore", data.scoreRisco ?? 0);
    setText("diagnosisRegion", data.regiao || findRegionName(state.selectedRegionId));
    setText("diagnosisSummary", details.summary);
    setText("diagnosisReason", details.reason);
    setText("diagnosisPriority", details.priority);
    setText("diagnosisDeadline", details.deadline);
    setText("diagnosisTechnical", details.technical);
    setText("diagnosisSource", data.fonteDados || "Open-Meteo API");
    document.getElementById("diagnosisActions").innerHTML = details.actions
        .map((action) => `<li>${escapeHtml(action)}</li>`)
        .join("");
    applyRiskBadge(document.getElementById("diagnosisBadge"), data.nivelRisco);
}

function normalizeDiagnosis(data) {
    const risk = String(data.nivelRisco || "").toUpperCase();
    const fallback = getDiagnosisFallback(risk, data.recomendacao);

    return {
        summary: data.resumoRisco || fallback.summary,
        reason: data.motivoRisco || fallback.reason,
        actions: Array.isArray(data.acoesRecomendadas) && data.acoesRecomendadas.length
            ? data.acoesRecomendadas
            : fallback.actions,
        priority: data.prioridade || fallback.priority,
        deadline: data.prazoSugerido || fallback.deadline,
        technical: data.observacaoTecnica ||
            "Recomendacao calculada com dados climaticos em tempo real, considerando temperatura, chuva, umidade e vento."
    };
}

function getDiagnosisFallback(risk, recommendation) {
    const settings = {
        ALTO: {
            summary: "Risco elevado para os cultivos monitorados.",
            reason: "As condicoes climaticas atuais exigem medidas preventivas para reduzir perdas.",
            priority: "Alta",
            deadline: "Acao imediata nas proximas 24 horas."
        },
        MODERADO: {
            summary: "Risco moderado que exige acompanhamento preventivo.",
            reason: "Os indicadores climaticos recomendam ajustes no manejo e monitoramento frequente.",
            priority: "Media",
            deadline: "Revisar o manejo nas proximas 48 horas."
        },
        BAIXO: {
            summary: "Risco baixo e condicoes favoraveis ao manejo planejado.",
            reason: "Os indicadores permanecem dentro das faixas de acompanhamento.",
            priority: "Baixa",
            deadline: "Manter acompanhamento no proximo ciclo de manejo."
        }
    };
    const current = settings[risk] || settings.MODERADO;

    return {
        ...current,
        actions: [
            recommendation ||
            "Manter o monitoramento climatico e verificar a umidade do solo antes do proximo manejo."
        ]
    };
}

async function loadHistory() {
    elements.historyTableBody.innerHTML = `
        <tr><td colspan="7" class="empty-state">
            <span class="spinner-border spinner-border-sm text-success"></span>
            Carregando historico...
        </td></tr>`;

    try {
        const data = await apiFetch("/api/Recomendacoes/historico-climatico");
        state.history = Array.isArray(data) ? data : [];
        state.historyPage = 1;
        renderHistory();
        return true;
    } catch (error) {
        console.error("Falha ao carregar o historico:", error);
        state.history = [];
        elements.historyTableBody.innerHTML = `
            <tr><td colspan="7" class="empty-state">
                <i class="bi bi-exclamation-circle"></i>
                Nao foi possivel carregar o historico.
            </td></tr>`;
        elements.historyCount.textContent = "0 registros";
        elements.historyPagination.innerHTML = "";
        return false;
    }
}

async function refreshHistoryWithFeedback() {
    setButtonLoading(elements.refreshHistory, true, "Atualizando...");
    const success = await loadHistory();
    setButtonLoading(elements.refreshHistory, false);
    showToast(
        success ? "Historico atualizado com sucesso." : "Nao foi possivel atualizar o historico.",
        success ? "success" : "error"
    );
}

function renderHistory() {
    const total = state.history.length;
    const totalPages = Math.max(1, Math.ceil(total / HISTORY_PAGE_SIZE));
    state.historyPage = Math.min(state.historyPage, totalPages);

    if (total === 0) {
        elements.historyTableBody.innerHTML = `
            <tr><td colspan="7" class="empty-state">
                <i class="bi bi-clock-history"></i>
                Nenhum diagnostico climatico registrado.
            </td></tr>`;
        elements.historyCount.textContent = "0 registros";
        elements.historyPagination.innerHTML = "";
        return;
    }

    const start = (state.historyPage - 1) * HISTORY_PAGE_SIZE;
    const pageItems = state.history.slice(start, start + HISTORY_PAGE_SIZE);
    elements.historyTableBody.innerHTML = pageItems.map((item) => `
        <tr class="history-row" data-history-id="${item.idRecomendacao}" tabindex="0"
            aria-label="Ver detalhes da recomendacao de ${escapeHtml(formatDate(item.data))}">
            <td>${formatDate(item.data)}</td>
            <td><strong>${formatNumber(item.temperatura, 1)}</strong> &deg;C</td>
            <td>${formatNumber(item.umidade, 0)}%</td>
            <td>${formatNumber(item.chuva, 1)} mm</td>
            <td>${formatNumber(item.velocidadeVento, 1)} km/h</td>
            <td><strong>${item.scoreRisco ?? 0}</strong></td>
            <td><span class="risk-badge ${getRiskClass(item.nivelRisco)}">${escapeHtml(item.nivelRisco || "--")}</span></td>
        </tr>
    `).join("");

    const end = Math.min(start + HISTORY_PAGE_SIZE, total);
    elements.historyCount.textContent = `Exibindo ${start + 1}-${end} de ${total} registros`;
    renderPagination(totalPages);
}

function handleHistoryInteraction(event) {
    if (event.type === "keydown" && !["Enter", " "].includes(event.key)) {
        return;
    }

    const row = event.target.closest("[data-history-id]");

    if (!row) {
        return;
    }

    if (event.type === "keydown") {
        event.preventDefault();
    }

    const item = state.history.find(
        (record) => String(record.idRecomendacao) === String(row.dataset.historyId)
    );

    if (item) {
        openHistoryDetail(item);
    }
}

function openHistoryDetail(item) {
    const orientation = item.orientacao ||
        "Orientacao detalhada indisponivel para este registro antigo.";
    elements.historyDetailBody.innerHTML = `
        <div class="history-detail-grid">
            ${historyDetailItem("Associacao", item.associacao || "Nao informada")}
            ${historyDetailItem("Cultivo", item.cultivo || "Nao informado")}
            ${historyDetailItem("Data", formatDate(item.data))}
            ${historyDetailItem("Score de risco", `${item.scoreRisco ?? 0}/100`)}
            <div class="history-detail-item">
                <span>Nivel de risco</span>
                <strong><span class="risk-badge ${getRiskClass(item.nivelRisco)}">${escapeHtml(item.nivelRisco || "--")}</span></strong>
            </div>
            ${historyDetailItem("Temperatura", `${formatNumber(item.temperatura, 1)} °C`)}
            ${historyDetailItem("Umidade", `${formatNumber(item.umidade, 0)}%`)}
            ${historyDetailItem("Chuva", `${formatNumber(item.chuva, 1)} mm`)}
            ${historyDetailItem("Vento", `${formatNumber(item.velocidadeVento, 1)} km/h`)}
            ${historyDetailItem("Fonte dos dados", item.fonteDados || "Open-Meteo API")}
        </div>
        <div class="history-orientation">
            <span>Orientacao completa</span>
            <p>${escapeHtml(orientation)}</p>
        </div>`;
    state.historyDetailModal?.show();
}

function historyDetailItem(label, value) {
    return `
        <div class="history-detail-item">
            <span>${escapeHtml(label)}</span>
            <strong>${escapeHtml(value)}</strong>
        </div>`;
}

function renderPagination(totalPages) {
    if (totalPages <= 1) {
        elements.historyPagination.innerHTML = "";
        return;
    }

    const pages = [createPageItem("Anterior", state.historyPage - 1, state.historyPage === 1)];

    for (let page = 1; page <= totalPages; page += 1) {
        pages.push(createPageItem(String(page), page, false, page === state.historyPage));
    }

    pages.push(createPageItem("Proxima", state.historyPage + 1, state.historyPage === totalPages));
    elements.historyPagination.innerHTML = pages.join("");
    elements.historyPagination.querySelectorAll("[data-page]").forEach((button) => {
        button.addEventListener("click", () => {
            state.historyPage = Number(button.dataset.page);
            renderHistory();
        });
    });
}

function createPageItem(label, page, disabled = false, active = false) {
    return `
        <li class="page-item ${disabled ? "disabled" : ""} ${active ? "active" : ""}">
            <button class="page-link" type="button" data-page="${page}" ${disabled ? "disabled" : ""}>
                ${label}
            </button>
        </li>`;
}

async function loadReferenceData() {
    const requests = [
        ["regions", "/api/Regioes"],
        ["associations", "/api/Associacoes"],
        ["crops", "/api/Cultivos"]
    ];

    const results = await Promise.allSettled(requests.map(([, path]) => apiFetch(path)));
    results.forEach((result, index) => {
        if (result.status === "fulfilled") {
            state[requests[index][0]] = Array.isArray(result.value) ? result.value : [];
        }
    });
}

async function handleManagementTab(event) {
    const tab = event.target.closest("[data-entity]");

    if (!tab) {
        return;
    }

    state.managementEntity = tab.dataset.entity;
    elements.managementTabs.querySelectorAll(".management-tab").forEach((button) => {
        button.classList.toggle("active", button === tab);
    });
    await loadManagementData(state.managementEntity);
}

async function loadManagementData(entity = state.managementEntity) {
    const config = entityConfig[entity];
    updateManagementHeading(config);

    if (config.available === false) {
        elements.managementTableWrapper.classList.add("d-none");
        elements.managementUnavailable.classList.remove("d-none");
        elements.newRecord.disabled = true;
        elements.refreshManagement.disabled = true;
        return true;
    }

    elements.managementTableWrapper.classList.remove("d-none");
    elements.managementUnavailable.classList.add("d-none");
    elements.newRecord.disabled = false;
    elements.refreshManagement.disabled = false;
    renderManagementLoading();

    try {
        const data = await apiFetch(config.endpoint);
        state[entity] = Array.isArray(data) ? data : [];
        renderManagementTable(entity);
        return true;
    } catch (error) {
        console.error(`Erro ao carregar ${config.title}:`, error);
        renderManagementError(config.title);
        return false;
    }
}

function updateManagementHeading(config) {
    elements.managementKicker.textContent = config.kicker;
    elements.managementEntityTitle.textContent = config.title;
    elements.managementDescription.textContent = config.description;
}

function renderManagementLoading() {
    elements.managementTableHead.innerHTML = "<th>Carregando</th>";
    elements.managementTableBody.innerHTML = `
        <tr><td class="empty-state">
            <span class="spinner-border spinner-border-sm text-success"></span>
            Carregando registros...
        </td></tr>`;
}

function renderManagementError(title) {
    elements.managementTableHead.innerHTML = `<th>${escapeHtml(title)}</th>`;
    elements.managementTableBody.innerHTML = `
        <tr><td class="empty-state">
            <i class="bi bi-exclamation-circle"></i>
            Nao foi possivel carregar os registros.
        </td></tr>`;
}

function renderManagementTable(entity) {
    const records = state[entity] || [];
    const table = getManagementTableDefinition(entity);
    elements.managementTableHead.innerHTML = table.headers
        .map((header) => `<th>${header}</th>`)
        .join("") + '<th class="text-end">Acoes</th>';

    if (records.length === 0) {
        elements.managementTableBody.innerHTML = `
            <tr><td colspan="${table.headers.length + 1}" class="empty-state">
                <i class="bi bi-inbox"></i>
                Nenhum registro cadastrado.
            </td></tr>`;
        return;
    }

    const idField = entityConfig[entity].idField;
    elements.managementTableBody.innerHTML = records.map((record) => `
        <tr>
            ${table.cells(record).map((cell) => `<td>${cell}</td>`).join("")}
            <td class="actions-cell">
                <button class="table-action" type="button" data-action="edit"
                    data-id="${record[idField]}" title="Editar">
                    <i class="bi bi-pencil"></i>
                </button>
                <button class="table-action is-danger" type="button" data-action="delete"
                    data-id="${record[idField]}" title="Excluir">
                    <i class="bi bi-trash3"></i>
                </button>
            </td>
        </tr>
    `).join("");
}

function getManagementTableDefinition(entity) {
    const definitions = {
        regions: {
            headers: ["Nome", "UF", "Latitude", "Longitude"],
            cells: (record) => [
                `<strong>${escapeHtml(record.nomeRegiao)}</strong>`,
                escapeHtml(record.ufRegiao),
                formatNumber(record.latitude, 4),
                formatNumber(record.longitude, 4)
            ]
        },
        associations: {
            headers: ["Nome", "Sigla", "Regiao", "CNPJ", "Login"],
            cells: (record) => [
                `<strong>${escapeHtml(record.nomeAssociacao)}</strong>`,
                escapeHtml(record.siglaAssociacao),
                escapeHtml(record.nomeRegiao || findRegionName(record.idRegiao)),
                escapeHtml(record.cnpj),
                escapeHtml(record.login)
            ]
        },
        crops: {
            headers: ["Nome", "Categoria", "Porte", "Colheita", "Vida util", "Intermitencia"],
            cells: (record) => [
                `<strong>${escapeHtml(record.nomeCultivo)}</strong>`,
                escapeHtml(record.categoriaCultivo),
                escapeHtml(record.porteCultivo),
                escapeHtml(record.tempoColheita ?? "--"),
                escapeHtml(record.vidaUtil ?? "--"),
                escapeHtml(record.intermitencia)
            ]
        },
        recommendations: {
            headers: ["Data", "Associacao", "Cultivo", "Tipo", "Orientacao"],
            cells: (record) => [
                formatDate(record.dataRecAsc),
                escapeHtml(record.nomeAssociacao || findAssociationName(record.idAssociacao)),
                escapeHtml(record.nomeCultivo || findCropName(record.idCultivo)),
                escapeHtml(record.tipoRecomendacao || "--"),
                `<span title="${escapeHtml(record.orientacao || "")}">${escapeHtml(truncate(record.orientacao, 60))}</span>`
            ]
        }
    };

    return definitions[entity];
}

async function refreshManagementWithFeedback() {
    setButtonLoading(elements.refreshManagement, true, "Atualizando...");
    const success = await loadManagementData(state.managementEntity);
    setButtonLoading(elements.refreshManagement, false);
    showToast(
        success ? "Lista atualizada com sucesso." : "Nao foi possivel atualizar a lista.",
        success ? "success" : "error"
    );
}

function handleManagementAction(event) {
    const button = event.target.closest("[data-action]");

    if (!button) {
        return;
    }

    const config = entityConfig[state.managementEntity];
    const record = state[state.managementEntity].find(
        (item) => String(item[config.idField]) === String(button.dataset.id)
    );

    if (!record) {
        showToast("Registro nao encontrado.", "warning");
        return;
    }

    if (button.dataset.action === "edit") {
        openEntityForm(record);
    } else {
        openDeleteConfirmation(record);
    }
}

function openEntityForm(record = null) {
    const entity = state.managementEntity;
    const config = entityConfig[entity];

    if (config.available === false) {
        showToast("CRUD de agricultores nao disponivel no backend.", "warning");
        return;
    }

    if (entity === "associations" && state.regions.length === 0) {
        showToast("Cadastre uma regiao antes de criar uma associacao.", "warning");
        return;
    }

    if (
        entity === "recommendations" &&
        (state.associations.length === 0 || state.crops.length === 0)
    ) {
        showToast("Cadastre uma associacao e um cultivo antes da recomendacao.", "warning");
        return;
    }

    state.editingId = record ? record[config.idField] : null;
    elements.entityModalKicker.textContent = config.kicker;
    elements.entityModalTitle.textContent = record
        ? `Editar ${config.singular}`
        : `Nova ${config.singular}`;
    elements.entityFormFields.innerHTML = buildEntityForm(entity, record || {});
    state.entityModal?.show();
}

function buildEntityForm(entity, record = {}) {
    if (entity === "regions") {
        return `
            ${formInput("nomeRegiao", "Nome da regiao", record.nomeRegiao, "text", 8, 'maxlength="50" placeholder="Pernambuco"')}
            ${formSelect("ufRegiao", "UF", northeastStates(), record.ufRegiao, 4)}
            <div class="col-12"><div class="form-text">Latitude e longitude sao preenchidas automaticamente pelo backend.</div></div>`;
    }

    if (entity === "associations") {
        return `
            ${formInput("nomeAssociacao", "Nome da associacao", record.nomeAssociacao, "text", 8, 'maxlength="120" placeholder="Cooperativa de Produtores do Sertao"')}
            ${formInput("siglaAssociacao", "Sigla", record.siglaAssociacao, "text", 4, 'maxlength="10" placeholder="COOPSERTA"')}
            ${formSelect("idRegiao", "Regiao", state.regions.map((item) => ({
                value: item.idRegiao,
                label: `${item.nomeRegiao} - ${item.ufRegiao}`
            })), record.idRegiao, 6)}
            ${formInput("cnpj", "CNPJ", record.cnpj, "text", 6, 'maxlength="14" placeholder="12345678000199"')}
            ${formInput("login", "Login", record.login, "text", 6, 'maxlength="30" placeholder="coopersertao"')}
            ${formInput("senha", record.idAssociacao ? "Nova senha" : "Senha", "", "password", 6, 'maxlength="60" placeholder="AgroNordeste123"')}
            ${record.idAssociacao ? '<div class="col-12"><div class="form-text">Por seguranca, informe a senha ao salvar a edicao.</div></div>' : ""}`;
    }

    if (entity === "crops") {
        return `
            ${formInput("nomeCultivo", "Nome do cultivo", record.nomeCultivo, "text", 6, 'maxlength="50" placeholder="Milho"')}
            ${formInput("categoriaCultivo", "Categoria", record.categoriaCultivo, "text", 6, 'maxlength="40" placeholder="Graos"')}
            ${formSelect("porteCultivo", "Porte", [
                { value: "ARBUSTO", label: "Arbusto" },
                { value: "RAIZ", label: "Raiz" },
                { value: "ARVORE", label: "Arvore" },
                { value: "HORTALICA", label: "Hortalica" }
            ], record.porteCultivo, 4)}
            ${formInput("tempoColheita", "Tempo de colheita", record.tempoColheita, "text", 4, 'maxlength="30" placeholder="120 dias"')}
            ${formInput("vidaUtil", "Vida util", record.vidaUtil, "text", 4, 'maxlength="30" placeholder="6 meses"')}
            ${formInput("intermitencia", "Intermitencia", record.intermitencia, "text", 12, 'maxlength="30" placeholder="Anual"')}`;
    }

    return `
        ${formInput("dataRecAsc", "Data da recomendacao", toDateTimeLocal(record.dataRecAsc), "datetime-local", 12)}
        ${formSelect("idAssociacao", "Associacao", state.associations.map((item) => ({
            value: item.idAssociacao,
            label: `${item.nomeAssociacao} (${item.siglaAssociacao})`
        })), record.idAssociacao, 6)}
        ${formSelect("idCultivo", "Cultivo", state.crops.map((item) => ({
            value: item.idCultivo,
            label: item.nomeCultivo
        })), record.idCultivo, 6)}
        <div class="col-12"><div class="form-text">Orientacao e tipo sao calculados automaticamente pelo backend.</div></div>`;
}

function formInput(name, label, value = "", type = "text", columns = 6, attributes = "") {
    return `
        <div class="col-md-${columns}">
            <label class="form-label" for="field-${name}">${label}</label>
            <input class="form-control" id="field-${name}" name="${name}" type="${type}"
                value="${escapeHtml(value ?? "")}" ${attributes} required>
        </div>`;
}

function formSelect(name, label, options, selectedValue, columns = 6) {
    return `
        <div class="col-md-${columns}">
            <label class="form-label" for="field-${name}">${label}</label>
            <select class="form-select" id="field-${name}" name="${name}" required>
                <option value="">Selecione...</option>
                ${options.map((option) => {
                    const item = typeof option === "string"
                        ? { value: option, label: option }
                        : option;
                    const selected = String(item.value) === String(selectedValue) ? "selected" : "";
                    return `<option value="${escapeHtml(item.value)}" ${selected}>${escapeHtml(item.label)}</option>`;
                }).join("")}
            </select>
        </div>`;
}

async function saveEntity(event) {
    event.preventDefault();

    if (!elements.entityForm.reportValidity()) {
        return;
    }

    const entity = state.managementEntity;
    const config = entityConfig[entity];
    const payload = collectEntityPayload(entity, new FormData(elements.entityForm));
    const isEditing = state.editingId !== null;
    const path = isEditing ? `${config.endpoint}/${state.editingId}` : config.endpoint;
    const method = isEditing ? "PUT" : "POST";
    setButtonLoading(elements.saveEntity, true, "Salvando...");

    try {
        await apiFetch(path, { method, body: JSON.stringify(payload) });
        state.entityModal?.hide();
        await refreshAfterMutation(entity);
        showToast(
            `${capitalize(config.singular)} ${isEditing ? "atualizada" : "cadastrada"} com sucesso.`,
            "success"
        );
    } catch (error) {
        console.error(`Erro ao salvar ${config.singular}:`, error);
        showToast(`Nao foi possivel salvar: ${error.message}`, "error");
    } finally {
        setButtonLoading(elements.saveEntity, false);
    }
}

function collectEntityPayload(entity, formData) {
    const value = (name) => String(formData.get(name) || "").trim();

    if (entity === "regions") {
        return { nomeRegiao: value("nomeRegiao"), ufRegiao: value("ufRegiao") };
    }

    if (entity === "associations") {
        return {
            nomeAssociacao: value("nomeAssociacao"),
            siglaAssociacao: value("siglaAssociacao"),
            idRegiao: Number(value("idRegiao")),
            cnpj: value("cnpj"),
            login: value("login"),
            senha: value("senha")
        };
    }

    if (entity === "crops") {
        return {
            nomeCultivo: value("nomeCultivo"),
            categoriaCultivo: value("categoriaCultivo"),
            porteCultivo: value("porteCultivo"),
            tempoColheita: value("tempoColheita"),
            vidaUtil: value("vidaUtil"),
            intermitencia: value("intermitencia")
        };
    }

    return {
        dataRecAsc: new Date(value("dataRecAsc")).toISOString(),
        idAssociacao: Number(value("idAssociacao")),
        idCultivo: Number(value("idCultivo"))
    };
}

function openDeleteConfirmation(record) {
    const config = entityConfig[state.managementEntity];
    state.deleteTarget = {
        entity: state.managementEntity,
        id: record[config.idField]
    };
    elements.deleteRecordName.textContent = getRecordLabel(state.managementEntity, record);
    state.deleteModal?.show();
}

async function deleteEntity() {
    if (!state.deleteTarget) {
        return;
    }

    const { entity, id } = state.deleteTarget;
    const config = entityConfig[entity];
    setButtonLoading(elements.confirmDelete, true, "Excluindo...");

    try {
        await apiFetch(`${config.endpoint}/${id}`, { method: "DELETE" });
        state.deleteModal?.hide();
        state.deleteTarget = null;
        await refreshAfterMutation(entity);
        showToast(`${capitalize(config.singular)} excluida com sucesso.`, "success");
    } catch (error) {
        console.error(`Erro ao excluir ${config.singular}:`, error);
        showToast(`Nao foi possivel excluir: ${error.message}`, "error");
    } finally {
        setButtonLoading(elements.confirmDelete, false);
    }
}

async function refreshAfterMutation(entity) {
    await Promise.all([loadManagementData(entity), loadDashboard(), loadReferenceData()]);

    if (entity === "regions") {
        await loadRegions();
    }

    if (entity === "recommendations") {
        await loadHistory();
    }
}

function getRecordLabel(entity, record) {
    const labels = {
        regions: `${record.nomeRegiao} - ${record.ufRegiao}`,
        associations: `${record.nomeAssociacao} (${record.siglaAssociacao})`,
        crops: record.nomeCultivo,
        recommendations: `Recomendacao de ${formatDate(record.dataRecAsc)}`
    };
    return labels[entity] || "Esta acao nao pode ser desfeita.";
}

function findRegionName(id) {
    return state.regions.find((item) => String(item.idRegiao) === String(id))?.nomeRegiao || "--";
}

function findAssociationName(id) {
    return state.associations.find(
        (item) => String(item.idAssociacao) === String(id)
    )?.nomeAssociacao || "--";
}

function findCropName(id) {
    return state.crops.find((item) => String(item.idCultivo) === String(id))?.nomeCultivo || "--";
}

function northeastStates() {
    return ["AL", "BA", "CE", "MA", "PB", "PE", "PI", "RN", "SE"];
}

function initializeChart() {
    const canvas = document.getElementById("climateChart");
    const chartColors = getChartThemeColors();

    if (typeof Chart === "undefined") {
        canvas.insertAdjacentHTML(
            "afterend",
            '<p class="text-muted small mt-3">Nao foi possivel carregar o Chart.js.</p>'
        );
        return;
    }

    state.chart = new Chart(canvas, {
        type: "bar",
        data: {
            labels: ["Temperatura", "Umidade", "Chuva", "Vento"],
            datasets: [{
                data: [0, 0, 0, 0],
                backgroundColor: ["#dc7650", "#3f91be", "#6e7fc2", "#64948b"],
                borderRadius: 8,
                borderSkipped: false,
                barPercentage: 0.66
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            animation: { duration: 650 },
            plugins: {
                legend: { display: false },
                tooltip: {
                    displayColors: false,
                    callbacks: {
                        label(context) {
                            const units = ["°C", "%", "mm", "km/h"];
                            return `${context.raw} ${units[context.dataIndex]}`;
                        }
                    }
                }
            },
            scales: {
                x: {
                    grid: { display: false },
                    border: { display: false },
                    ticks: { color: chartColors.text, font: { size: 10, weight: 600 } }
                },
                y: {
                    beginAtZero: true,
                    suggestedMax: 100,
                    border: { display: false },
                    grid: { color: chartColors.grid },
                    ticks: { color: chartColors.text, font: { size: 10 } }
                }
            }
        }
    });
}

function updateChartTheme() {
    if (!state.chart) {
        return;
    }

    const chartColors = getChartThemeColors();
    state.chart.options.scales.x.ticks.color = chartColors.text;
    state.chart.options.scales.y.ticks.color = chartColors.text;
    state.chart.options.scales.y.grid.color = chartColors.grid;
    state.chart.update();
}

function getChartThemeColors() {
    const styles = getComputedStyle(document.documentElement);
    return {
        grid: styles.getPropertyValue("--chart-grid").trim() || "#edf1ee",
        text: styles.getPropertyValue("--chart-text").trim() || "#6d7971"
    };
}

function updateChart(data) {
    if (!state.chart) {
        return;
    }

    state.chart.data.datasets[0].data = [
        Number(data.temperatura) || 0,
        Number(data.umidade) || 0,
        Number(data.chuva) || 0,
        Number(data.velocidadeVento) || 0
    ];
    state.chart.update();
}

function setClimateLoading(isLoading) {
    elements.regionSelect.disabled = isLoading || state.regions.length === 0;
    elements.consultClimate.disabled = isLoading || !state.selectedRegionId;
    elements.generateDiagnosis.disabled = isLoading || !state.selectedRegionId;

    if (isLoading) {
        ["temperatureValue", "humidityValue", "rainValue", "windValue", "riskScore"]
            .forEach((id) => setText(id, "..."));
    }
}

function setDiagnosisLoading(isLoading) {
    elements.generateDiagnosis.disabled = isLoading || !state.selectedRegionId;
    elements.generateDiagnosis.querySelector(".button-content").classList.toggle("d-none", isLoading);
    elements.generateDiagnosis.querySelector(".button-loading").classList.toggle("d-none", !isLoading);
}

function setButtonLoading(button, isLoading, loadingLabel = "Carregando...") {
    if (!button) {
        return;
    }

    if (!button.dataset.defaultHtml) {
        button.dataset.defaultHtml = button.innerHTML;
    }

    button.disabled = isLoading;
    button.innerHTML = isLoading
        ? `<span class="spinner-border spinner-border-sm"></span> ${loadingLabel}`
        : button.dataset.defaultHtml;
}

function applyRiskBadge(element, riskLevel) {
    const normalized = String(riskLevel || "").toUpperCase();
    element.className = `risk-badge ${getRiskClass(normalized)}`;
    element.textContent = normalized || "--";
}

function getRiskClass(riskLevel) {
    const normalized = String(riskLevel || "").toUpperCase();

    if (normalized === "ALTO") return "risk-high";
    if (normalized === "MODERADO") return "risk-medium";
    if (normalized === "BAIXO") return "risk-low";
    return "risk-neutral";
}

function getRiskDescription(riskLevel) {
    const normalized = String(riskLevel || "").toUpperCase();

    if (normalized === "ALTO") return "Atencao imediata recomendada";
    if (normalized === "MODERADO") return "Monitoramento preventivo";
    if (normalized === "BAIXO") return "Condicoes sob controle";
    return "Aguardando dados";
}

function setIntegrationStatus(isOnline) {
    elements.openMeteoStatus.classList.toggle("offline", !isOnline);
    elements.openMeteoStatus.querySelector("span:last-child").textContent =
        isOnline ? "Open-Meteo Online" : "Open-Meteo Indisponivel";
}

function showToast(message, type = "success") {
    const icons = {
        success: "bi-check-circle-fill",
        error: "bi-x-circle-fill",
        warning: "bi-exclamation-triangle-fill"
    };
    const toast = document.createElement("div");
    toast.className = `toast app-toast toast-${type}`;
    toast.setAttribute("role", "status");
    toast.innerHTML = `
        <div class="toast-body">
            <i class="bi ${icons[type] || icons.success}"></i>
            <span>${escapeHtml(message)}</span>
            <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Fechar"></button>
        </div>`;
    elements.toastContainer.appendChild(toast);

    if (typeof bootstrap !== "undefined") {
        const instance = new bootstrap.Toast(toast, { delay: 4500 });
        toast.addEventListener("hidden.bs.toast", () => toast.remove());
        instance.show();
    } else {
        toast.classList.add("show");
        window.setTimeout(() => toast.remove(), 4500);
    }
}

function showGlobalError(message) {
    elements.globalAlert.textContent = message;
    elements.globalAlert.classList.remove("d-none");
    window.clearTimeout(showGlobalError.timeout);
    showGlobalError.timeout = window.setTimeout(() => {
        elements.globalAlert.classList.add("d-none");
    }, 8000);
}

function updateTimestamp() {
    elements.lastUpdate.textContent = new Intl.DateTimeFormat("pt-BR", {
        hour: "2-digit",
        minute: "2-digit"
    }).format(new Date());
}

function setText(id, value) {
    const element = document.getElementById(id);

    if (element) {
        element.textContent = value ?? "--";
        element.classList.remove("placeholder-glow");
    }
}

function formatNumber(value, decimalPlaces = 1) {
    const number = Number(value);

    if (!Number.isFinite(number)) {
        return "--";
    }

    return new Intl.NumberFormat("pt-BR", {
        minimumFractionDigits: decimalPlaces,
        maximumFractionDigits: decimalPlaces
    }).format(number);
}

function formatDate(value) {
    const date = new Date(value);

    if (Number.isNaN(date.getTime())) {
        return "--";
    }

    return new Intl.DateTimeFormat("pt-BR", {
        dateStyle: "short",
        timeStyle: "short"
    }).format(date);
}

function toDateTimeLocal(value) {
    const date = value ? new Date(value) : new Date();

    if (Number.isNaN(date.getTime())) {
        return "";
    }

    const offset = date.getTimezoneOffset();
    return new Date(date.getTime() - offset * 60000).toISOString().slice(0, 16);
}

function truncate(value, maxLength) {
    const text = String(value || "");
    return text.length > maxLength ? `${text.slice(0, maxLength - 1)}...` : text;
}

function capitalize(value) {
    const text = String(value || "");
    return text ? text.charAt(0).toUpperCase() + text.slice(1) : text;
}

function clamp(value, min, max) {
    return Math.min(Math.max(Number(value) || 0, min), max);
}

function escapeHtml(value) {
    const element = document.createElement("div");
    element.textContent = String(value ?? "");
    return element.innerHTML;
}
