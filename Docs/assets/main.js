(() => {
  const PHASES = [
    {
      id: 1,
      title: "Getting Started",
      group: "foundation",
      summary: "Layered architecture, DTO mapping, and clean project structure.",
      concepts: ["layered architecture", "DTO mapping", "project structure"],
      complexity: "O(n) list operations, O(1) lookups",
      effort: { junior: "4 to 6h", intermediate: "2 to 4h", expert: "1 to 2h" }
    },
    {
      id: 2,
      title: "Scaffolding",
      group: "foundation",
      summary: "Expand to full CRUD with command and query separation.",
      concepts: ["CQRS basics", "CRUD coverage", "request mapping"],
      complexity: "O(n) scans, O(1) key lookup",
      effort: { junior: "6 to 8h", intermediate: "4 to 6h", expert: "2 to 3h" }
    },
    {
      id: 3,
      title: "Validation and Pagination",
      group: "foundation",
      summary: "Fluent validation, filtering, search, and pagination quality.",
      concepts: ["validation", "filtering", "pagination"],
      complexity: "filter O(n), sort O(n log n), page O(k)",
      effort: { junior: "8 to 12h", intermediate: "5 to 8h", expert: "3 to 5h" }
    },
    {
      id: 4,
      title: "Standards and Error Handling",
      group: "foundation",
      summary: "Shared coding standards and robust API error behavior.",
      concepts: ["error handling", "coding standards", "API consistency"],
      complexity: "pipeline O(m), routing O(1)",
      effort: { junior: "6 to 10h", intermediate: "4 to 6h", expert: "2 to 4h" }
    },
    {
      id: 5,
      title: "Performance Improvement",
      group: "performance",
      summary: "Caching and compression for predictable response speed.",
      concepts: ["response caching", "compression", "performance profiling"],
      complexity: "cache hit O(1), miss O(n)",
      effort: { junior: "6 to 10h", intermediate: "4 to 6h", expert: "2 to 4h" }
    },
    {
      id: 6,
      title: "Events and Background Jobs",
      group: "performance",
      summary: "Domain events and background scheduling flows.",
      concepts: ["domain events", "notification flow", "Hangfire jobs"],
      complexity: "fan out O(h), scheduled jobs O(j)",
      effort: { junior: "8 to 12h", intermediate: "5 to 8h", expert: "3 to 5h" }
    },
    {
      id: 7,
      title: "Microservices and Client Integration",
      group: "services",
      summary: "Generated clients and service-to-service communication.",
      concepts: ["NSwag clients", "service boundaries", "integration contracts"],
      complexity: "orchestration O(s) service calls",
      effort: { junior: "8 to 14h", intermediate: "6 to 10h", expert: "4 to 6h" }
    },
    {
      id: 8,
      title: "Security",
      group: "services",
      summary: "Authentication, authorization, and secure-by-default patterns.",
      concepts: ["JWT auth", "OAuth2", "secure headers"],
      complexity: "auth checks O(1), policy O(p)",
      effort: { junior: "8 to 14h", intermediate: "6 to 10h", expert: "4 to 6h" }
    },
    {
      id: 9,
      title: "User Interface",
      group: "services",
      summary: "User-facing journey integration with API backends.",
      concepts: ["MVC or Blazor", "UI integration", "end-to-end flow"],
      complexity: "render O(v), request O(r)",
      effort: { junior: "10 to 16h", intermediate: "7 to 12h", expert: "5 to 8h" }
    },
    {
      id: 10,
      title: "Database Migrations",
      group: "cloud",
      summary: "Schema change control using DbUp migration scripts.",
      concepts: ["DbUp", "schema evolution", "versioned scripts"],
      complexity: "migration execution O(scripts)",
      effort: { junior: "8 to 14h", intermediate: "6 to 10h", expert: "4 to 6h" }
    },
    {
      id: 11,
      title: "Cloud Native Orchestration",
      group: "cloud",
      summary: "Aspire orchestration with observability and coordination.",
      concepts: ["Aspire", "OpenTelemetry", "service discovery"],
      complexity: "orchestration O(services)",
      effort: { junior: "10 to 16h", intermediate: "7 to 12h", expert: "5 to 8h" }
    },
    {
      id: 12,
      title: "Custom Dispatcher Pattern",
      group: "advanced",
      summary: "Project-specific CQRS dispatcher and handler pipeline control.",
      concepts: ["dispatcher flow", "handler mapping", "pipeline control"],
      complexity: "dispatch O(1), chain O(h)",
      effort: { junior: "10 to 16h", intermediate: "7 to 12h", expert: "5 to 8h" }
    },
    {
      id: 13,
      title: "MCP and AI Integration",
      group: "advanced",
      summary: "Expose domain tools via MCP for AI-assisted workflows.",
      concepts: ["MCP server", "tool contracts", "AI integration"],
      complexity: "tool dispatch O(t), request O(r)",
      effort: { junior: "10 to 16h", intermediate: "7 to 12h", expert: "5 to 8h" }
    },
    {
      id: 14,
      title: "External API Integration",
      group: "advanced",
      summary: "Reliable third-party API and webhook integration patterns.",
      concepts: ["typed HttpClient", "retry strategy", "webhook handling"],
      complexity: "resilience O(retries)",
      effort: { junior: "8 to 14h", intermediate: "6 to 10h", expert: "4 to 6h" }
    },
    {
      id: 15,
      title: "Container Publishing",
      group: "advanced",
      summary: "Container build and publish flow with CI pipelines.",
      concepts: ["Docker build", "GHCR publish", "release automation"],
      complexity: "pipeline O(steps)",
      effort: { junior: "8 to 14h", intermediate: "6 to 10h", expert: "4 to 6h" }
    }
  ];

  const STORAGE_KEY = "incubator-learning-validation-v1";
  const defaultState = {
    evidence: {},
    quizPassByPhase: {},
    wholeQuizPass: false
  };

  const state = loadState();

  const phaseGrid = document.getElementById("phase-grid");
  const search = document.getElementById("search");
  const roleFocus = document.getElementById("role-focus");
  const pathMode = document.getElementById("path-mode");
  const jumpSection = document.getElementById("jump-section");
  const validationSummary = document.getElementById("validation-summary");

  const quizMode = document.getElementById("quiz-mode");
  const quizPhase = document.getElementById("quiz-phase");
  const quizArea = document.getElementById("quiz-area");
  const startQuizButton = document.getElementById("start-quiz");
  const submitQuizButton = document.getElementById("submit-quiz");
  const quizResult = document.getElementById("quiz-result");

  let currentQuiz = null;

  if (!phaseGrid || !search || !roleFocus || !pathMode || !jumpSection || !validationSummary || !quizMode || !quizPhase || !quizArea || !startQuizButton || !submitQuizButton || !quizResult) {
    return;
  }

  populateQuizPhase();
  render();
  updateSummary();

  search.addEventListener("input", render);
  roleFocus.addEventListener("change", render);
  pathMode.addEventListener("change", render);
  jumpSection.addEventListener("change", render);

  quizMode.addEventListener("change", () => {
    quizPhase.disabled = quizMode.value === "whole";
    clearQuiz();
  });

  startQuizButton.addEventListener("click", startQuiz);
  submitQuizButton.addEventListener("click", submitQuiz);

  function loadState() {
    try {
      const raw = window.localStorage.getItem(STORAGE_KEY);
      if (!raw) {
        return { ...defaultState };
      }
      const parsed = JSON.parse(raw);
      return {
        evidence: parsed.evidence || {},
        quizPassByPhase: parsed.quizPassByPhase || {},
        wholeQuizPass: Boolean(parsed.wholeQuizPass)
      };
    } catch (_error) {
      return { ...defaultState };
    }
  }

  function saveState() {
    window.localStorage.setItem(STORAGE_KEY, JSON.stringify(state));
  }

  function normalize(value) {
    return String(value || "").toLowerCase().trim();
  }

  function getGuideLink(phaseId) {
    return `https://github.com/entelect-incubator/.NET/tree/master/Phase%20${phaseId}`;
  }

  function getEvidence(phaseId) {
    const existing = state.evidence[String(phaseId)] || {};
    return {
      repo: Boolean(existing.repo),
      build: Boolean(existing.build),
      run: Boolean(existing.run)
    };
  }

  function setEvidence(phaseId, patch) {
    const key = String(phaseId);
    state.evidence[key] = { ...getEvidence(phaseId), ...patch };
    saveState();
    updateSummary();
    render();
  }

  function isPhaseValidated(phaseId) {
    const evidence = getEvidence(phaseId);
    const hasEvidence = evidence.repo && evidence.build && evidence.run;
    const hasQuiz = Boolean(state.quizPassByPhase[String(phaseId)]);
    return hasEvidence && hasQuiz;
  }

  function filteredPhases() {
    const query = normalize(search.value);
    const selectedRole = roleFocus.value;
    const mode = pathMode.value;
    const selectedSection = jumpSection.value;

    return PHASES.filter((phase) => {
      const roleCheck = selectedRole === "all" || Boolean(phase.effort[selectedRole]);
      const sectionCheck = mode === "whole" || selectedSection === "all" || phase.group === selectedSection;
      const searchText = normalize([phase.id, phase.title, phase.summary, phase.complexity, phase.concepts.join(" "), phase.effort.junior, phase.effort.intermediate, phase.effort.expert].join(" "));
      const queryCheck = !query || searchText.includes(query);
      return roleCheck && sectionCheck && queryCheck;
    });
  }

  function render() {
    const phases = filteredPhases();
    phaseGrid.innerHTML = "";

    phases.forEach((phase) => {
      const evidence = getEvidence(phase.id);
      const card = document.createElement("article");
      card.className = "phase-card";

      const passedQuiz = Boolean(state.quizPassByPhase[String(phase.id)]);
      const validated = isPhaseValidated(phase.id);

      card.innerHTML = `
        <div class="phase-head">
          <h3>Phase ${phase.id}: ${phase.title}</h3>
          <span class="state-pill ${validated ? "done" : "todo"}">${validated ? "Validated" : "Not validated"}</span>
        </div>
        <p>${phase.summary}</p>
        <p class="complexity"><strong>Time complexity focus:</strong> ${phase.complexity}</p>
        <div class="effort-grid">
          <span><strong>Junior:</strong> ${phase.effort.junior}</span>
          <span><strong>Intermediate:</strong> ${phase.effort.intermediate}</span>
          <span><strong>Expert:</strong> ${phase.effort.expert}</span>
        </div>
        <div class="concept-list">
          ${phase.concepts.map((concept) => `<span class="concept-chip">${concept}</span>`).join("")}
        </div>
        <div class="evidence-checks">
          <label><input type="checkbox" data-phase="${phase.id}" data-evidence="repo" ${evidence.repo ? "checked" : ""}> Repo created or forked</label>
          <label><input type="checkbox" data-phase="${phase.id}" data-evidence="build" ${evidence.build ? "checked" : ""}> Build evidence captured</label>
          <label><input type="checkbox" data-phase="${phase.id}" data-evidence="run" ${evidence.run ? "checked" : ""}> Run or feature evidence captured</label>
        </div>
        <div class="actions">
          <a href="${getGuideLink(phase.id)}" target="_blank" rel="noopener">Open phase guide</a>
          <button type="button" data-quiz-phase="${phase.id}">Take phase quiz</button>
          <span class="quiz-pill ${passedQuiz ? "done" : "todo"}">${passedQuiz ? "Quiz 100%" : "Quiz pending"}</span>
        </div>
      `;

      phaseGrid.appendChild(card);
    });

    phaseGrid.querySelectorAll("input[type='checkbox'][data-evidence]").forEach((checkbox) => {
      checkbox.addEventListener("change", (event) => {
        const target = event.target;
        const phaseId = Number(target.getAttribute("data-phase"));
        const evidenceKey = target.getAttribute("data-evidence");
        setEvidence(phaseId, { [evidenceKey]: target.checked });
      });
    });

    phaseGrid.querySelectorAll("button[data-quiz-phase]").forEach((button) => {
      button.addEventListener("click", () => {
        const phaseId = Number(button.getAttribute("data-quiz-phase"));
        quizMode.value = "phase";
        quizPhase.disabled = false;
        quizPhase.value = String(phaseId);
        startQuiz();
        document.getElementById("validation").scrollIntoView({ behavior: "smooth", block: "start" });
      });
    });
  }

  function updateSummary() {
    const validatedCount = PHASES.filter((phase) => isPhaseValidated(phase.id)).length;
    const allPhasesDone = validatedCount === PHASES.length;
    const incubatorComplete = allPhasesDone && state.wholeQuizPass;

    validationSummary.innerHTML = `
      <p><strong>Phase validation:</strong> ${validatedCount} of ${PHASES.length} phases complete.</p>
      <p><strong>Whole-incubator quiz:</strong> ${state.wholeQuizPass ? "Passed at 100%" : "Pending"}.</p>
      <p><strong>Incubator completion status:</strong> ${incubatorComplete ? "Complete" : "In progress"}.</p>
    `;
  }

  function populateQuizPhase() {
    quizPhase.innerHTML = "";
    PHASES.forEach((phase) => {
      const option = document.createElement("option");
      option.value = String(phase.id);
      option.textContent = `Phase ${phase.id}: ${phase.title}`;
      quizPhase.appendChild(option);
    });
  }

  function sampleOtherConcept(correctConcept, phaseId) {
    const pool = PHASES.filter((phase) => phase.id !== phaseId)
      .flatMap((phase) => phase.concepts)
      .filter((concept) => concept !== correctConcept);
    const items = [];
    while (items.length < 3 && pool.length > 0) {
      const index = Math.floor(Math.random() * pool.length);
      const pick = pool.splice(index, 1)[0];
      if (!items.includes(pick)) {
        items.push(pick);
      }
    }
    return items;
  }

  function buildQuestionsForPhase(phase) {
    const concept = phase.concepts[0];
    const conceptOptions = shuffle([concept, ...sampleOtherConcept(concept, phase.id)]);

    const effortQuestion = {
      id: `effort-${phase.id}`,
      prompt: `What is the intermediate effort estimate for Phase ${phase.id}?`,
      options: shuffle([phase.effort.intermediate, "1 to 2h", "14 to 20h", "20 to 30h"]),
      answer: phase.effort.intermediate
    };

    const complexityQuestion = {
      id: `complexity-${phase.id}`,
      prompt: `Choose the time complexity focus for Phase ${phase.id}.`,
      options: shuffle([
        phase.complexity,
        "constant rendering O(1) only",
        "always quadratic O(n^2)",
        "no complexity consideration"
      ]),
      answer: phase.complexity
    };

    const conceptQuestion = {
      id: `concept-${phase.id}`,
      prompt: `Which concept belongs to Phase ${phase.id}?`,
      options: conceptOptions,
      answer: concept
    };

    const evidenceQuestion = {
      id: `evidence-${phase.id}`,
      prompt: "Which item is mandatory evidence for validation?",
      options: shuffle([
        "Build evidence captured",
        "Only reading the README",
        "Only watching a video",
        "Only opening the repository"
      ]),
      answer: "Build evidence captured"
    };

    return [effortQuestion, complexityQuestion, conceptQuestion, evidenceQuestion];
  }

  function buildQuestionsForWhole() {
    const selected = shuffle([...PHASES]).slice(0, 10);
    return selected.map((phase) => {
      const concept = phase.concepts[0];
      const options = shuffle([concept, ...sampleOtherConcept(concept, phase.id)]);
      return {
        id: `whole-${phase.id}`,
        prompt: `Pick a core concept from Phase ${phase.id}: ${phase.title}.`,
        options,
        answer: concept
      };
    });
  }

  function startQuiz() {
    clearQuiz();

    const mode = quizMode.value;
    if (mode === "phase") {
      const phaseId = Number(quizPhase.value);
      const phase = PHASES.find((item) => item.id === phaseId);
      if (!phase) {
        return;
      }
      currentQuiz = {
        mode,
        phaseId,
        questions: buildQuestionsForPhase(phase)
      };
    } else {
      currentQuiz = {
        mode,
        phaseId: null,
        questions: buildQuestionsForWhole()
      };
    }

    renderQuiz(currentQuiz.questions);
    submitQuizButton.disabled = false;
    quizResult.textContent = "";
  }

  function clearQuiz() {
    currentQuiz = null;
    quizArea.innerHTML = "";
    submitQuizButton.disabled = true;
    quizResult.textContent = "";
  }

  function renderQuiz(questions) {
    quizArea.innerHTML = "";
    questions.forEach((question, index) => {
      const wrapper = document.createElement("fieldset");
      wrapper.className = "question";
      wrapper.innerHTML = `
        <legend>${index + 1}. ${question.prompt}</legend>
        ${question.options
          .map(
            (option, optionIndex) => `
          <label>
            <input type="radio" name="q-${question.id}" value="${escapeHtml(option)}" ${optionIndex === 0 ? "" : ""}>
            ${option}
          </label>
        `
          )
          .join("")}
      `;
      quizArea.appendChild(wrapper);
    });
  }

  function submitQuiz() {
    if (!currentQuiz) {
      return;
    }

    let correct = 0;
    const total = currentQuiz.questions.length;

    currentQuiz.questions.forEach((question) => {
      const selected = quizArea.querySelector(`input[name="q-${question.id}"]:checked`);
      const selectedValue = selected ? selected.value : "";
      if (selectedValue === question.answer) {
        correct += 1;
      }
    });

    if (correct === total) {
      if (currentQuiz.mode === "phase" && currentQuiz.phaseId) {
        state.quizPassByPhase[String(currentQuiz.phaseId)] = true;
      }
      if (currentQuiz.mode === "whole") {
        state.wholeQuizPass = true;
      }
      saveState();
      quizResult.textContent = `Pass: ${correct}/${total}. Required 100 percent achieved.`;
      quizResult.classList.remove("fail");
      quizResult.classList.add("pass");
      updateSummary();
      render();
      return;
    }

    quizResult.textContent = `Fail: ${correct}/${total}. You need 100 percent to pass. Retake required.`;
    quizResult.classList.remove("pass");
    quizResult.classList.add("fail");
  }

  function shuffle(items) {
    const copy = [...items];
    for (let i = copy.length - 1; i > 0; i -= 1) {
      const j = Math.floor(Math.random() * (i + 1));
      [copy[i], copy[j]] = [copy[j], copy[i]];
    }
    return copy;
  }

  function escapeHtml(value) {
    return String(value)
      .replaceAll("&", "&amp;")
      .replaceAll("<", "&lt;")
      .replaceAll(">", "&gt;")
      .replaceAll('"', "&quot;")
      .replaceAll("'", "&#39;");
  }
})();
