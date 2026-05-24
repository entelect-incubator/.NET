# 🤖 AI Instructions for Migration Tasks

**Purpose**: Guidelines for AI assistants creating migration packages  
**Date**: October 30, 2025  
**Scope**: All migration tasks (framework, library, pattern migrations)  

---

## 📋 Migration Package Structure

### Standard Files to Create

Every migration package should include:

1. **README (Quick Start)** - 300-500 lines
   - 60-second overview
   - 4-step process with exact commands
   - Before/after code (2-3 examples)
   - Common errors (5-8 items)
   - Success checklist

2. **Complete Guide** - 1,000-1,500 lines
   - Interface/pattern mappings (tables)
   - File organization strategy
   - Namespace transformation rules
   - ~20+ before/after code examples
   - DI configuration changes
   - Controller/endpoint updates
   - NuGet package changes
   - File renaming checklist

3. **Master Reference** - 1,500-2,000 lines
   - Global mapping table
   - File organization patterns
   - Namespace transformation rules
   - Code pattern replacements (detailed)
   - Using statement updates
   - Phase-by-phase inventory
   - DI configuration mapping
   - Execution order (phases)
   - Quick find & replace reference
   - Rollback procedure

4. **Scope Inventory** - 500-1,000 lines
   - Current state analysis
   - Target state design
   - File counts by category
   - Commands/handlers/services/controllers breakdown
   - Validator/behavior mappings
   - DI changes specific to scope
   - Task checklist

5. **Executive Summary** - 750-1,000 lines
   - Package overview
   - Detailed timeline
   - Complete checklist
   - Pre-migration validation
   - Success criteria (8-10 points)
   - Troubleshooting guide
   - Next steps
   - Support resources

6. **Navigation Index** - 500-800 lines
   - Document guide by purpose
   - Document guide by role
   - Document guide by topic
   - Search tree for common questions
   - Cross-reference map
   - Quick links section

7. **Code Template** - 200-400 lines
   - Drop-in configuration code
   - Handler/service examples
   - Controller pattern
   - DI setup examples
   - Migration checklist (inline)
   - Before/after comparison

8. **Completion Status** - 300-500 lines
   - What's included summary
   - Scope coverage breakdown
   - Quick start instructions
   - File manifest
   - Success metrics
   - Next steps

### Optional Files

9. **Automation Script** - 400-1000 lines (if applicable)
   - PowerShell or Python
   - Backup creation
   - File reorganization
   - Bulk search/replace
   - Dry-run mode
   - Report generation
   - Error handling

---

## 🎯 Migration Analysis Framework

Before creating migration package, analyze:

### 1. Current State
```
What exists today?
- Which interfaces/patterns are being used?
- How are files organized?
- What's the dependency structure?
- Are there ~X files/phases/modules?
```

### 2. Target State
```
What should exist after migration?
- New interfaces/patterns to use
- New file organization
- New dependency structure
- Timeline for phases
```

### 3. Scope
```
How big is this migration?
- ~X files affected
- ~Y developers involved
- ~Z hours estimated
- N phases
```

### 4. Complexity
```
What makes this challenging?
- Deep coupling issues?
- Cross-phase dependencies?
- Breaking changes?
- API compatibility concerns?
```

### 5. Risks
```
What could go wrong?
- Build failures?
- Runtime issues?
- Data corruption?
- Performance degradation?
```

---

## 📝 Documentation Pattern

### For Each Document, Include

```markdown
# Document Title

**Purpose**: What this document explains  
**Status**: ✅ Complete | 🚧 In Progress | ⏳ Not Started  
**Date**: Creation date  
**Scope**: What it covers  

---

## Table of Contents

[Include if >20 pages]

---

## Quick Summary

[60-second overview if >10 pages]

---

## Main Content

[Organized by sections]

---

## Related Documentation

[Links to other docs in package]

---

## Support Tree / Navigation

[Search by topic/error/role]

---

**Version**: 1.0  
**Last Updated**: Date  
**Status**: Status  
```

---

## 📊 Code Examples Pattern

For every interface/pattern change, show:

```csharp
// BEFORE (old way)
namespace Feature.Commands;

public class CreateXyzCommand : IOldInterface<T>
{
    public async Task Handle(...)
    {
        // Implementation
    }
}

// AFTER (new way)
namespace Feature.Commands.Handlers;  // Note: namespace changed

public class CreateXyzCommandHandler : INewInterface<T>
{
    public async Task HandleAsync(...)  // Note: method renamed
    {
        // Implementation (logic same, signature changed)
    }
}
```

### Code Example Guidelines

- Show 3-line context before and after
- Highlight what changed (comments with "Note:")
- Show at least 2-3 examples per pattern
- Include controller/DI/configuration examples
- Provide complete copy-paste code when possible
- Include comments explaining the why

---

## 📋 Checklist Pattern

Every guide should have actionable checklists:

```markdown
## Pre-Migration Checklist

- [ ] Git committed: `git status` shows clean
- [ ] Tool available: `command --version` output
- [ ] Documentation read: Understand the plan
- [ ] Time available: 2-4 hours uninterrupted
- [ ] Backup strategy: Know how to rollback
```

---

## 🎯 Execution Pattern

For "big bang" migrations:

```markdown
## Execution Strategy

### Phase 1: Preparation (5 min)
- Create backup
- Verify tools
- Review checklist

### Phase 2: Automation (10 min)
- Run migration script
- Review report
- Validate structure

### Phase 3: Compilation (1-2 hours)
- dotnet build (will fail)
- Fix error #1, build
- Fix error #2, build
- Repeat until success

### Phase 4: Validation (30 min)
- Update DI configuration
- dotnet test
- dotnet run
- Verify endpoints
```

---

## 🔍 Analysis Document Pattern

For inventory/scope documents:

```markdown
## Current State Analysis

| Category           | Count | Status          |
| ------------------ | ----- | --------------- |
| Commands           | X     | IRequest<T>     |
| Queries            | Y     | IRequest<T>     |
| Handlers           | Z     | Nested in files |
| Validators         | W     | Existing        |
| Controllers        | V     | Using IMediator |

## Target State Design

| Category           | Count | Status                |
| ------------------ | ----- | --------------------- |
| Commands           | X     | ICommand<T>           |
| Queries            | Y     | IQuery<T>             |
| Handlers           | Z     | Separate in Handlers/ |
| Validators         | W     | In Validators/        |
| Controllers        | V     | Using ICommandMediator|

## Migration Impact

- Files affected: X
- Lines changed: Y
- Breaking changes: Z
- Phases: N
```

---

## 🎓 Table of Contents Pattern

Create TOC for documents >1000 lines:

```markdown
## 📋 Table of Contents

1. [Quick Overview](#quick-overview)
2. [Interface Mapping](#interface-mapping)
   - [Commands](#commands-write-operations)
   - [Queries](#queries-read-operations)
   - [Handlers](#handlers)
3. [File Organization](#file-organization-strategy)
4. [Code Examples](#code-pattern-replacements)
5. [Checklists](#checklists)
6. [Troubleshooting](#troubleshooting)
7. [Next Steps](#next-steps)

---
```

---

## 🔗 Cross-Linking Pattern

Link between all migration documents:

### In README
```markdown
## Related Documentation

- **Complete Guide**: [MIGRATION-PRODUCT-GUIDE.md](MIGRATION-PRODUCT-GUIDE.md)
- **Reference Mapping**: [REFACTORING-MAPPING.md](REFACTORING-MAPPING.md)
- **Phase Analysis**: [PHASE9-FILE-INVENTORY.md](PHASE9-FILE-INVENTORY.md)
- **Timeline**: [MIGRATION-EXECUTIVE-SUMMARY.md](MIGRATION-EXECUTIVE-SUMMARY.md)
- **Navigation**: [INDEX-PRODUCT-MIGRATION.md](INDEX-PRODUCT-MIGRATION.md)
```

### In each guide
```markdown
## Quick Links

| Question                | Find In                    |
| ----------------------- | -------------------------- |
| "Give me the 4 steps"   | README-PRODUCT-MIGRATION   |
| "Show me code examples" | MIGRATION-PRODUCT-GUIDE    |
| "What about Phase 9?"   | PHASE9-FILE-INVENTORY      |
| "Full timeline?"        | MIGRATION-EXECUTIVE-SUMMARY|
| "All mappings?"         | REFACTORING-MAPPING        |
```

---

## ✨ Key Content Rules

### DO ✅

1. **Be Specific**
   - Show actual commands: `dotnet build`, not "build project"
   - Show actual file paths: `Core/Pizza/Commands/CreatePizzaCommand.cs`
   - Show actual interfaces: `ICommandHandler<T,R>`, not "handler interface"

2. **Show Before/After**
   - Every interface change: show old and new
   - Every file move: show old and new location
   - Every code pattern: show old implementation and new

3. **Provide Context**
   - Why is this changing?
   - What problem does it solve?
   - What's the benefit?

4. **Make it Actionable**
   - Include copy-paste code where possible
   - Provide search/replace patterns
   - Give exact commands to run

5. **Organize for Navigation**
   - Use clear section headers
   - Create table of contents
   - Link between documents
   - Cross-reference patterns

6. **Test Everything**
   - Verify commands actually work
   - Validate code examples compile
   - Ensure paths are correct
   - Check links are valid

### DON'T ✗

1. **Don't be vague**
   - ✗ "Update the files" → ✓ "Update CreatePizzaCommand.cs: line 5"
   - ✗ "Fix the errors" → ✓ "Remove the 'using MediatR;' statement"

2. **Don't skip steps**
   - ✗ "Run the migration" → ✓ "cd d:\Dev\...; .\script.ps1 -DryRun $true"

3. **Don't overload one document**
   - ✗ 2000+ line single-purpose doc
   - ✓ 500 lines per document, multiple docs

4. **Don't leave things uncertain**
   - ✗ "Might need to update Program.cs"
   - ✓ "Update Program.cs: replace 'AddMediatR(...)' with 'AddLiteBus(...)'"

5. **Don't create orphan documents**
   - ✗ 1 guide document with no quick start or index
   - ✓ README + Guide + Index + Reference

---

## 📊 Document Quality Checklist

Before finalizing migration package, verify:

### Content Quality
- [ ] 60-second overview provided
- [ ] 4-step process documented
- [ ] All interfaces mapped
- [ ] Before/after code examples (3+ per pattern)
- [ ] File structure documented (current vs target)
- [ ] Namespace changes explained
- [ ] DI configuration covered
- [ ] ~25+ code examples total

### Organization
- [ ] Quick start (README) created
- [ ] Detailed guide created
- [ ] Master reference created
- [ ] Scope inventory created
- [ ] Executive summary created
- [ ] Navigation index created
- [ ] All documents linked
- [ ] Table of contents in large docs

### Validation
- [ ] All commands tested
- [ ] All code examples valid
- [ ] All file paths correct
- [ ] All links working
- [ ] Checklists complete
- [ ] No typos or grammar errors
- [ ] Consistent formatting
- [ ] Professional appearance

### Automation
- [ ] Dry-run mode available
- [ ] Backup creation automated
- [ ] Error handling implemented
- [ ] Report generation included
- [ ] Comments explaining logic
- [ ] Usage examples provided

### Completeness
- [ ] ~680-5800 lines documentation
- [ ] 5-9 files total
- [ ] All phases covered
- [ ] All file types covered
- [ ] All error types covered
- [ ] All user roles covered

---

## 🎯 Success Metrics

A complete migration package achieves:

| Metric                      | Target | Status |
| --------------------------- | ------ | ------ |
| Quick start page            | 1      | ✓      |
| Detailed guides             | 1-2    | ✓      |
| Reference documents         | 2-3    | ✓      |
| Code examples               | 20+    | ✓      |
| Automation scripts          | 1+     | ✓      |
| Total documentation lines   | 5,000+ | ✓      |
| Cross-references            | 30+    | ✓      |
| Checklists                  | 5+     | ✓      |
| Troubleshooting items       | 10+    | ✓      |
| Time to complete migration  | 2-4h   | ✓      |
| User satisfaction           | High   | ✓      |

---

## 🔐 Best Practices

### For Documentation

1. **Start with README** - Everything else builds from there
2. **Use tables** - Easier to scan than paragraphs
3. **Show code** - Examples are worth 1000 words
4. **Link everything** - Make navigation easy
5. **Add context** - Why, not just what
6. **Include checklists** - Make progress visible

### For Automation

1. **Always have dry-run** - Never change without preview
2. **Create backups** - Always restore option
3. **Generate reports** - Show what changed
4. **Handle errors** - Don't fail silently
5. **Test thoroughly** - Before users run it
6. **Document logic** - Future maintainers will appreciate

### For Organization

1. **Keep files grouped** - By purpose/task
2. **Use consistent naming** - MIGRATION-X-Y.md pattern
3. **Create index** - Help users find things
4. **Archive completed** - Clean up old tasks
5. **Maintain versions** - Track changes over time

---

## 📞 AI Assistant Workflow for Migrations

When creating a migration package:

```
1. ANALYZE
   ├─ Understand current architecture
   ├─ Define target architecture
   ├─ Identify scope (files, phases)
   └─ List breaking changes

2. PLAN
   ├─ Create mapping (old → new)
   ├─ Design file structure
   ├─ Plan execution order
   └─ Identify automation opportunities

3. DOCUMENT (primary task)
   ├─ Create README (quick start)
   ├─ Create detailed guide
   ├─ Create reference mapping
   ├─ Create scope inventory
   ├─ Create executive summary
   ├─ Create navigation index
   └─ Create completion status

4. AUTOMATE
   ├─ Design PowerShell/Python script
   ├─ Implement bulk changes
   ├─ Add dry-run mode
   ├─ Add reporting
   ├─ Add error handling
   └─ Test thoroughly

5. VALIDATE
   ├─ Verify all commands
   ├─ Test code examples
   ├─ Check all links
   ├─ Verify file paths
   └─ Check cross-references

6. PACKAGE
   ├─ Organize files
   ├─ Update INDEX
   ├─ Create manifest
   ├─ Add AI instructions
   └─ Mark complete

7. DELIVER
   ├─ Create docs/tasks/<migration>/
   ├─ Copy/link all files
   ├─ Update TASK-INDEX.md
   ├─ Add to git
   └─ Provide summary to user
```

---

## 📝 Template: Migration Package Manifest

Include in each migration package:

```markdown
## Package Manifest

**Migration**: Old Framework → New Framework  
**Date**: YYYY-MM-DD  
**Status**: ✅ Ready | 🚧 In Progress  
**Version**: 1.0  

### Files Included

- [ ] README-PRODUCT-MIGRATION.md (quick start)
- [ ] MIGRATION-PRODUCT-GUIDE.md (detailed guide)
- [ ] REFACTORING-MAPPING.md (master reference)
- [ ] PHASE9-FILE-INVENTORY.md (scope analysis)
- [ ] MIGRATION-EXECUTIVE-SUMMARY.md (timeline)
- [ ] INDEX-PRODUCT-MIGRATION.md (navigation)
- [ ] PACKAGE-COMPLETE.md (completion status)
- [ ] Migrate-Product.ps1 (automation script)

### Statistics

- Documentation lines: X,XXX+
- Code examples: YY+
- Files affected: ~ZZ0+
- Estimated time: H hours
- Phases: N

### Quality Metrics

- ✅ All commands tested
- ✅ All code examples valid
- ✅ All links working
- ✅ Full coverage
- ✅ Professional quality
```

---

**Version**: 1.0  
**Last Updated**: October 30, 2025  
**Status**: ✅ Ready for Use  

Use these guidelines to create migration packages that are comprehensive, well-organized, and user-friendly.

---
