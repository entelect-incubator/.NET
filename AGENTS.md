# AI Agents Guide — Working with Multiple AI Tools

This guide explains how to work effectively with different AI agents (GitHub Copilot, ChatGPT, Claude, etc.) in the Pezza project, including their strengths, weaknesses, and best practices for each.

---

## 🤖 Overview of AI Tools

### GitHub Copilot
**Best for:** In-editor code completion, quick snippets, real-time suggestions

**Strengths:**
- Context-aware from current file
- Integrated in VS Code
- Real-time suggestions as you type
- Learns from your codebase patterns
- Great for extending existing patterns

**Weaknesses:**
- Limited context (one file at a time)
- Can't see project structure
- Occasionally suggests outdated patterns
- Requires more human guidance

**Best Use Cases:**
- Completing method bodies
- Generating repetitive code (tests, mappers)
- Extending existing patterns
- Filling in boilerplate

---

### ChatGPT / GPT-4
**Best for:** Conceptual questions, large refactoring, code review

**Strengths:**
- Excellent at explaining concepts
- Great for planning architecture
- Good at refactoring suggestions
- Can analyze large code blocks
- Conversational for iterative refinement

**Weaknesses:**
- No direct access to your codebase
- Token limit (you must paste code)
- Can't run code
- May suggest generic solutions

**Best Use Cases:**
- Understanding CQRS and design patterns
- Code review assistance
- Refactoring strategies
- Architecture planning
- Troubleshooting complex issues

---

### Claude (Anthropic)
**Best for:** Complex code analysis, long-context tasks, architectural decisions

**Strengths:**
- Excellent code reasoning
- Handles large code files well (200K+ tokens)
- Great at explaining trade-offs
- Conservative (less likely to suggest wrong patterns)
- Superior at architectural discussions

**Weaknesses:**
- Slower response time
- Can't see your codebase directly
- Not integrated into editors (yet)
- Requires manual copying

**Best Use Cases:**
- Analyzing entire code systems
- Complex bug analysis
- Architectural guidance
- Security review
- Performance optimization strategies

---

## 📋 Choosing the Right Tool

### Task Matrix

| Task                        | Copilot | ChatGPT | Claude |
| --------------------------- | ------- | ------- | ------ |
| Quick method generation     | ⭐⭐⭐     | ⭐⭐      | ⭐⭐     |
| Full handler implementation | ⭐⭐⭐     | ⭐⭐⭐     | ⭐⭐⭐    |
| Test generation             | ⭐⭐⭐     | ⭐⭐⭐     | ⭐⭐⭐    |
| Understanding concepts      | ⭐⭐      | ⭐⭐⭐     | ⭐⭐⭐    |
| Code review                 | ⭐⭐      | ⭐⭐⭐     | ⭐⭐⭐    |
| Bug troubleshooting         | ⭐⭐      | ⭐⭐⭐     | ⭐⭐⭐    |
| Architecture planning       | ⭐       | ⭐⭐⭐     | ⭐⭐⭐    |
| Refactoring advice          | ⭐⭐      | ⭐⭐⭐     | ⭐⭐⭐    |
| Real-time editing           | ⭐⭐⭐     | ❌       | ❌      |
| Large file analysis         | ⭐⭐      | ⭐⭐      | ⭐⭐⭐    |

---

## 🛠️ Tool-Specific Workflows

### Workflow 1: Generate a New Handler (Copilot)

1. **Create handler class signature** in VS Code:
   ```csharp
   public class GetPizzasQueryHandler(DatabaseContext databaseContext) 
       : IRequestHandler<GetPizzasQuery, ListResult<PizzaModel>>
   {
       public async Task<ListResult<PizzaModel>> Handle(
           GetPizzasQuery request,
           CancellationToken cancellationToken)
   ```

2. **Let Copilot suggest** the body (press `Ctrl+Shift+A` or wait for suggestion)

3. **Review** line-by-line:
   - Does it use primary constructor correctly?
   - Are null checks present?
   - Is `CancellationToken` passed to async calls?
   - Does it use `.Map()` for conversion?
   - Does it return `Result<T>`?

4. **Accept/reject** individual suggestions:
   - Good line? Press `Tab` to accept
   - Bad suggestion? Press `Esc` and type it yourself

5. **Refactor** if needed:
   - Move duplicated code to extensions
   - Add caching if it's a query
   - Verify error handling

**Typical Copilot Issues to Watch:**
- ❌ Suggests `entity.Model` instead of `entity.Map()`
- ❌ Forgets `CancellationToken` on async methods
- ❌ Suggests `.Result` instead of `await`
- ❌ Missing null checks

---

### Workflow 2: Code Review with ChatGPT

1. **Copy the handler** to ChatGPT (keep context minimal):
   ```
   CONTEXT:
   - Project uses CQRS with MediatR
   - All handlers use primary constructors
   - Return Result<T> for all operations
   - All async methods receive CancellationToken
   
   HANDLER TO REVIEW:
   [paste your code]
   
   REVIEW FOCUS:
   - Does it follow primary constructor pattern?
   - Are null checks present?
   - Is error handling correct?
   - Any DRY violations?
   ```

2. **Ask specific questions:**
   ```
   "Is there anything I could refactor to an extension method?
   Should this be cached?"
   ```

3. **Iterate** on suggestions:
   - "How would you refactor this for caching?"
   - "Can you explain why that's better?"

**Typical ChatGPT Issues:**
- ⚠️ May suggest patterns outside your architecture
- ⚠️ Might not know your specific project structure
- ⚠️ Could suggest different naming conventions

**Solutions:**
- Provide architecture context upfront
- Reference your documentation
- Correct it gently if suggestions don't fit

---

### Workflow 3: Complex Bug Analysis with Claude

1. **Describe the problem** clearly:
   ```
   ISSUE:
   GetPizzasQuery handler is slow (5+ seconds) on large datasets
   
   RELEVANT CODE:
   [paste handler]
   [paste query]
   [paste database context setup]
   
   ENVIRONMENT:
   - .NET 10
   - EF Core
   - In-memory database with 10K+ pizzas
   - No indexing
   
   QUESTIONS:
   1. What's causing the slowdown?
   2. How should I optimize this?
   3. Should I add caching here?
   ```

2. **Provide code context**:
   - Handler implementation
   - Related queries/commands
   - Database schema (if relevant)
   - Configuration

3. **Ask for trade-offs**:
   - "What's the trade-off between caching and database optimization?"
   - "Should I paginate or cache?"

4. **Implement suggestions** carefully:
   - Test with your actual data
   - Verify against your architecture
   - Benchmark improvements

**Claude Excel At:**
- Deep analysis of complex problems
- Trade-off discussions
- Performance optimization strategies
- Security implications
- Architectural guidance

---

## 🎯 Common Scenarios & Recommended Tools

### Scenario 1: I need to create a new command handler quickly
**Recommended:** Copilot
- Type the class signature
- Let Copilot suggest
- Review and refactor in 2-3 minutes

### Scenario 2: I'm not sure how to structure this feature
**Recommended:** ChatGPT or Claude
- Describe what you want to build
- Ask for architectural guidance
- Discuss trade-offs
- Then use Copilot to implement

### Scenario 3: My code is slow
**Recommended:** Claude
- Paste relevant code
- Describe performance issue
- Get analysis and optimization suggestions
- Use Copilot to implement fixes

### Scenario 4: I need to understand CQRS better
**Recommended:** ChatGPT (or documentation)
- Ask "Explain CQRS in the context of MediatR"
- Get a tutorial-style explanation
- Ask follow-up questions

### Scenario 5: I'm refactoring to reduce duplication
**Recommended:** ChatGPT or Claude
- Paste duplicated code sections
- Ask for refactoring suggestions
- Get options for extensions, mappers, behaviors
- Choose approach and use Copilot to implement

### Scenario 6: I need comprehensive test coverage
**Recommended:** Copilot + ChatGPT
- Use Copilot for test structure (happy path)
- Use ChatGPT to brainstorm test cases
- Use Copilot to generate remaining tests

### Scenario 7: Migrating legacy code to new architecture
**Recommended:** Claude (for planning) + Copilot (for implementation)
- Claude: Analyze old code, suggest refactoring path
- Copilot: Generate new handler structures
- ChatGPT: Help with specific refactoring decisions

---

## 💡 Best Practices for Multi-Tool Workflows

### 1. Establish Context First (All Tools)
**Before asking anything**, provide:
```
PROJECT CONTEXT:
- Architecture: CQRS with MediatR
- Framework: .NET 10
- Key Pattern: Primary constructors, Result<T>, Entity mapping
- Team Standard: No underscores in properties
- Domain: Pizza ordering system

CURRENT FILE/TASK:
[describe what you're working on]
```

### 2. Use the Right Tool for the Right Task
- **Stuck on syntax?** → Copilot (fastest)
- **Confused about architecture?** → ChatGPT (clearest explanation)
- **Need deep analysis?** → Claude (best reasoning)
- **Want multiple perspectives?** → Ask all three, then decide

### 3. Iterative Refinement with ChatGPT/Claude
```
FIRST REQUEST:
"Generate a handler for GetPizzas with filtering"

FOLLOW-UP 1:
"Can you add caching with 12-hour expiry?"

FOLLOW-UP 2:
"How would you handle the cache invalidation in the UpdatePizza handler?"

FOLLOW-UP 3:
"Can you show me extension methods for the filtering?"
```

### 4. Copilot: Accept Good Suggestions Faster
- Review in chunks (5-10 lines)
- Accept `Tab` if it matches your patterns
- Reject `Esc` if it's wrong
- Don't overthink — refactor later

### 5. Combining Tools for Complex Tasks

**Example: Complete Feature Implementation**

```
STEP 1 - Architecture (Claude):
"I need to create a feature to search pizzas by name, price range, 
and availability. Should I use a single Query with filtering, or 
multiple queries? What about caching strategy?"

STEP 2 - Planning (ChatGPT):
"Given the architecture decision, generate:
1. Request/Response DTOs
2. Handler structure outline
3. Extension methods needed
4. Test cases to cover"

STEP 3 - Implementation (Copilot):
- Create DTOs from ChatGPT outline
- Copilot generates handler body
- Copilot generates extension methods
- Copilot generates test stubs

STEP 4 - Review (ChatGPT/Claude):
"Review this implementation for:
- DRY violations
- Performance issues
- Test coverage gaps"
```

---

## ⚙️ Configuring Copilot for Pezza

### VS Code Settings
```json
{
  "github.copilot.advanced": {
    "debug.overrideCacheForContextRetrieval": false,
    "debug.testOverrideProblems": false,
    "debug.overrideToken": null,
    "authProvider": "github"
  }
}
```

### Tips for Better Copilot Suggestions
1. **Keep files focused** — Single domain per file
2. **Use clear naming** — Copilot learns from your names
3. **Write comments** — Explains intent:
   ```csharp
   // Filter pizzas by name (case-insensitive)
   var filtered = cachedData.FilterByName(search.Name);
   ```
4. **Show examples** — Add an example handler in the file
5. **Create patterns** — Use consistent structure

---

## 🔄 Multi-Tool Development Loop

### Complete Feature Implementation

```
1. DESIGN (Claude)
   ↓
   "What's the best architecture for [feature]?"
   
2. PLANNING (ChatGPT)
   ↓
   "Generate test cases and structure outline"
   
3. CODING (Copilot)
   ↓
   "Fill in the implementation quickly"
   
4. REVIEW (ChatGPT/Claude)
   ↓
   "Is this following all the patterns? Any issues?"
   
5. REFACTOR (Copilot)
   ↓
   "Extract this to an extension method"
   
6. TEST (Copilot + ChatGPT)
   ↓
   "Generate remaining test cases"
   
7. COMMIT
```

---

## ⚠️ When NOT to Use AI

- ❌ **Security-critical code** without human review
- ❌ **Complex business logic** without architect review
- ❌ **Database migrations** without data backup
- ❌ **Performance-critical paths** without benchmarking
- ❌ **Code you don't understand** — Always understand before committing

---

## 📊 Effectiveness Metrics

### Good Signs You're Using AI Well
- ✅ Code passes all tests
- ✅ Follows team standards (no underscores, primary constructors, etc.)
- ✅ Implements all requirements
- ✅ Similar to existing patterns in codebase
- ✅ You understand every line committed

### Red Flags
- 🚩 Code doesn't follow naming conventions
- 🚩 You don't understand what AI suggested
- 🚩 Tests are failing
- 🚩 Code violates architecture principles
- 🚩 You can't explain your code to a reviewer

**If red flags appear:** Don't commit. Ask the AI to fix it, or fix it yourself.

---

## 🎓 Learning Resources

### For Understanding AI Better
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [ChatGPT Best Practices](https://platform.openai.com/docs/guides/prompt-engineering)
- [Claude System Prompt Guide](https://docs.anthropic.com/en/docs/build-a-claude-app/system-prompts)

### For Prompting Better
- [OpenAI Prompt Engineering Guide](https://platform.openai.com/docs/guides/prompt-engineering)
- [Anthropic Prompt Engineering](https://docs.anthropic.com/en/docs/build-a-claude-app/prompt-engineering)

### Pezza-Specific Resources
- [COPILOT-INSTRUCTIONS.md](./COPILOT-INSTRUCTIONS.md) — System prompt for Copilot
- [DEVELOP_WITH_AI.md](./DEVELOP_WITH_AI.md) — Complete development guide
- [AI_QUICK_REFERENCE.md](./AI_QUICK_REFERENCE.md) — One-page standards

---

## 🤝 Team Agreements

### When Using AI on Team Project
1. **Always verify** code follows team standards
2. **Understand** what you're committing
3. **Test** thoroughly before PR
4. **Reference** the AI guide in code reviews
5. **Share** prompts that worked well
6. **Flag** patterns AI suggests that don't fit

### In Code Reviews
- "Does this match COPILOT-INSTRUCTIONS.md?"
- "Can you explain why AI suggested this approach?"
- "Let's extract this to an extension method"
- "Did you test all the edge cases?"

### Sharing Knowledge
- Found a great prompt? Share it
- Hit a limitation? Document it
- Discovered a pattern? Add it to guides
- Got stuck? Ask the team

---

## 📞 Quick Reference

**Need quick code?** → GitHub Copilot (fastest)
**Need explanation?** → ChatGPT (clearest)
**Need analysis?** → Claude (deepest)
**Need multiple views?** → Ask all three, pick best answer

---

**Last Updated**: October 30, 2025  
**For**: Pezza Pizza Ordering System  
**Framework**: .NET 10.0  
**Related Docs**: DEVELOP_WITH_AI.md, COPILOT-INSTRUCTIONS.md, AI_QUICK_REFERENCE.md
