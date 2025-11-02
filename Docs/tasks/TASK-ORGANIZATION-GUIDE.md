# 📋 Task Document Organization Guide

**Purpose**: How to organize and manage task-related markdown files in the future  
**Date**: October 30, 2025  
**Status**: Guide for future migrations and tasks  

---

## 📁 Folder Structure

```
docs/
├── tasks/                              # All task-related documentation
│   ├── migrations/                     # Migration task files
│   │   ├── README-LITEBUS-MIGRATION.md
│   │   ├── MIGRATION-LITEBUS-GUIDE.md
│   │   ├── MIGRATION-EXECUTIVE-SUMMARY.md
│   │   ├── PHASE9-FILE-INVENTORY.md
│   │   ├── REFACTORING-MAPPING.md
│   │   ├── INDEX-LITEBUS-MIGRATION.md
│   │   └── PACKAGE-COMPLETE.md
│   ├── archived/                       # Completed/archived tasks
│   │   └── (completed tasks moved here)
│   ├── TASK-ORGANIZATION-GUIDE.md     # This file
│   └── TASK-INDEX.md                  # Index of all tasks
├── memory/
├── spell/
└── ...
```

---

## 📝 File Naming Convention

### Migration Tasks

```
MIGRATION-<TYPE>-<DESCRIPTION>.md
PHASE-<NUMBER>-<DESCRIPTION>.md
<DESCRIPTION>-MAPPING.md
<DESCRIPTION>-INVENTORY.md
<DESCRIPTION>-SUMMARY.md
README-<TASK>-<TYPE>.md
INDEX-<TASK>-<TYPE>.md
PACKAGE-<TASK>-STATUS.md
```

### Examples

✅ **MIGRATION-LITEBUS-GUIDE.md** - Migration guide for LiteBus  
✅ **PHASE9-FILE-INVENTORY.md** - Inventory for Phase 9 files  
✅ **REFACTORING-MAPPING.md** - Mapping for refactoring  
✅ **README-LITEBUS-MIGRATION.md** - Quick start for migration  
✅ **INDEX-LITEBUS-MIGRATION.md** - Navigation index  
✅ **MIGRATION-EXECUTIVE-SUMMARY.md** - Executive summary  

---

## 🗂️ Task Categories

### Current Tasks

| Category     | Folder       | Files                         | Status     |
| ------------ | ------------ | ----------------------------- | ---------- |
| Migrations   | `migrations/` | LiteBus migration (8 files)   | ✅ Complete |
| Archived     | `archived/`   | (for completed tasks)         | 📁 Ready    |

### Future Task Organization

When creating new task folders:

```
docs/tasks/<task-type>/
├── README-<TASK>-<TYPE>.md              # Quick start (1 page)
├── <TASK>-GUIDE.md                      # Comprehensive guide
├── <TASK>-MAPPING.md                    # Reference mapping
├── <TASK>-INVENTORY.md                  # Analysis/inventory
├── <TASK>-SUMMARY.md                    # Executive summary
├── <TASK>-CHECKLIST.md                  # Task checklist
├── INDEX-<TASK>.md                      # Navigation
└── PACKAGE-COMPLETE.md                  # Completion status
```

---

## 🎯 Task File Types

### 1. Quick Start (README)
- **Purpose**: One-page overview
- **Filename**: `README-<TASK>-<TYPE>.md`
- **Audience**: First-time users
- **Content**: 4-step process, quick examples, links to full docs
- **Length**: 5-10 pages
- **Example**: `README-LITEBUS-MIGRATION.md`

### 2. Comprehensive Guide
- **Purpose**: Detailed reference
- **Filename**: `<TASK>-GUIDE.md` or `MIGRATION-<PRODUCT>-GUIDE.md`
- **Audience**: Developers doing the work
- **Content**: Interfaces, patterns, before/after, code examples
- **Length**: 30-50 pages
- **Example**: `MIGRATION-LITEBUS-GUIDE.md`

### 3. Master Mapping/Reference
- **Purpose**: Complete mapping and reference
- **Filename**: `<TASK>-MAPPING.md` or `REFACTORING-MAPPING.md`
- **Audience**: During implementation for lookups
- **Content**: Tables, patterns, code replacements, all phases
- **Length**: 50-80 pages
- **Example**: `REFACTORING-MAPPING.md`

### 4. Inventory/Analysis
- **Purpose**: Analysis of specific scope
- **Filename**: `<SCOPE>-FILE-INVENTORY.md` or `<PHASE>-<ASPECT>-INVENTORY.md`
- **Audience**: For validation and verification
- **Content**: File counts, current state, target state, checklist
- **Length**: 20-40 pages
- **Example**: `PHASE9-FILE-INVENTORY.md`

### 5. Executive Summary
- **Purpose**: High-level overview with timeline
- **Filename**: `<TASK>-EXECUTIVE-SUMMARY.md` or `MIGRATION-<PRODUCT>-EXECUTIVE-SUMMARY.md`
- **Audience**: Project managers, decision makers
- **Content**: Timeline, scope, costs, success criteria
- **Length**: 20-30 pages
- **Example**: `MIGRATION-EXECUTIVE-SUMMARY.md`

### 6. Index/Navigation
- **Purpose**: Help find right document
- **Filename**: `INDEX-<TASK>.md`
- **Audience**: Anyone using the package
- **Content**: Navigation, cross-references, search tree
- **Length**: 15-25 pages
- **Example**: `INDEX-LITEBUS-MIGRATION.md`

### 7. Completion Status
- **Purpose**: Package summary
- **Filename**: `PACKAGE-<TASK>-COMPLETE.md` or `PACKAGE-COMPLETE.md`
- **Audience**: Final review
- **Content**: What's included, scope covered, next steps
- **Length**: 10-15 pages
- **Example**: `PACKAGE-COMPLETE.md`

### 8. AI Instructions (NEW)
- **Purpose**: AI assistant guidelines for task
- **Filename**: `docs/tasks/AI-INSTRUCTIONS-<TASK>.md`
- **Audience**: AI assistants working on migrations/tasks
- **Content**: Patterns, guidelines, do's and don'ts, workflows
- **Length**: 10-20 pages
- **Example**: `AI-INSTRUCTIONS-MIGRATIONS.md`

---

## 🚀 Future Migration Template

When starting a new migration task, create this structure:

```
docs/tasks/<new-migration>/

1. README-<PRODUCT>-MIGRATION.md       (Day 1 start here)
   ↓
2. MIGRATION-<PRODUCT>-GUIDE.md        (Day 1 deep understanding)
   ↓
3. <PRODUCT>-FILE-INVENTORY.md         (Scope analysis)
   ↓
4. REFACTORING-MAPPING.md              (Reference during work)
   ↓
5. MIGRATION-EXECUTIVE-SUMMARY.md      (Timeline & checklist)
   ↓
6. INDEX-<PRODUCT>-MIGRATION.md        (Navigation)
   ↓
7. LITEBUS-STARTUP-TEMPLATE.cs         (Code templates)
   ↓
8. Migrate-<Old>-To-<New>.ps1          (Automation script)
   ↓
9. PACKAGE-COMPLETE.md                 (Final status)
```

---

## 📌 AI Instructions for Migrations

Create: `docs/tasks/AI-INSTRUCTIONS-MIGRATIONS.md`

This file should contain:
1. Migration task patterns
2. Common refactoring approaches
3. Documentation structure for migrations
4. Code organization principles
5. Before/after code patterns
6. Testing strategies
7. Automation script guidelines
8. Error handling patterns

---

## 🗂️ Moving Files After This Migration

### Current Location
```
d:\Dev\Incubator\.NET\
├── MIGRATION-LITEBUS-GUIDE.md
├── README-LITEBUS-MIGRATION.md
├── REFACTORING-MAPPING.md
├── PHASE9-FILE-INVENTORY.md
├── MIGRATION-EXECUTIVE-SUMMARY.md
├── INDEX-LITEBUS-MIGRATION.md
├── PACKAGE-COMPLETE.md
├── LITEBUS-STARTUP-TEMPLATE.cs
└── scripts/
    └── Migrate-MediatRToLiteBus.ps1
```

### New Location (for organization)
```
d:\Dev\Incubator\.NET\docs\tasks\migrations\
├── README-LITEBUS-MIGRATION.md           ✅ (copied)
├── MIGRATION-LITEBUS-GUIDE.md            → Link to parent
├── REFACTORING-MAPPING.md                → Link to parent
├── PHASE9-FILE-INVENTORY.md              → Link to parent
├── MIGRATION-EXECUTIVE-SUMMARY.md        → Link to parent
├── INDEX-LITEBUS-MIGRATION.md            → Link to parent
└── PACKAGE-COMPLETE.md                   → Link to parent
```

**Recommendation**: Keep originals in root for easy access, add copies + links in docs/tasks for organization.

---

## ✨ When to Archive Tasks

Move task files to `archived/` when:

- ✅ Task is 100% complete and verified
- ✅ Code has been deployed to production
- ✅ No more active work on this task
- ✅ New task folder has been created (if applicable)

**Archive Process**:
```powershell
# Move folder to archived
Move-Item -Path "docs/tasks/<task-name>" -Destination "docs/tasks/archived/<task-name>-COMPLETED-YYYY-MM-DD"

# Create index entry
# Add to docs/tasks/archived/INDEX.md
```

---

## 📑 TASK-INDEX.md Template

Create: `docs/tasks/TASK-INDEX.md`

```markdown
# Task Index

**Purpose**: Navigate all task-related documentation  
**Last Updated**: YYYY-MM-DD  

## Active Tasks

### Migrations

| Task              | Location                       | Status | Files |
| ----------------- | ------------------------------ | ------ | ----- |
| MediatR→LiteBus   | migrations/                    | ✅      | 8     |

## Archived Tasks

(None yet)

## Task Organization

See: TASK-ORGANIZATION-GUIDE.md
```

---

## 🎓 AI Migration Workflow

For AI assistants working on migrations, follow this workflow:

1. **Analyze** - Understand current architecture
2. **Plan** - Create migration strategy (big-bang vs incremental)
3. **Document** - Create migration package (7-9 files)
4. **Automate** - Build PowerShell/Python scripts (if needed)
5. **Validate** - Test with Phase N (usually Phase 9)
6. **Package** - Complete deliverables
7. **Organize** - Place in docs/tasks/<migration>/
8. **Archive** - Move to archived/ when complete

---

## 🔐 Rules for Task Files

### DO ✅

- ✅ Keep migration files organized in docs/tasks/
- ✅ Create quick-start (README) + detailed guides
- ✅ Include multiple entry points (quick/deep/reference)
- ✅ Provide before/after code examples
- ✅ Create automation scripts where possible
- ✅ Generate comprehensive checklists
- ✅ Make documents cross-referenced
- ✅ Include navigation index
- ✅ Add AI instructions for future work
- ✅ Archive completed tasks

### DON'T ✗

- ✗ Mix different task types in same folder
- ✗ Create task docs without quick-start
- ✗ Skip the automation opportunities
- ✗ Leave files scattered in root directory
- ✗ Forget about documentation
- ✗ Miss cross-references and linking
- ✗ Create tasks without checklists

---

## 📊 Task File Statistics

### LiteBus Migration Package

| Item              | Count |
| ----------------- | ----- |
| Markdown files    | 7     |
| Code templates    | 2     |
| Automation scripts| 1     |
| Total lines       | 5,800+|
| Code examples     | 25+   |

### Future Tasks Should Include

| Item              | Target |
| ----------------- | ------ |
| README file       | 1      |
| Guide file(s)     | 1-2    |
| Reference files   | 1-2    |
| Automation script | 1+     |
| Total files       | 5-9    |

---

## 🎯 Implementation Checklist

For organizing tasks in docs/tasks:

- [ ] Create main task folder: `docs/tasks/<task-name>/`
- [ ] Create README file (quick start)
- [ ] Create detailed guide(s)
- [ ] Create reference/mapping file
- [ ] Create inventory/analysis file
- [ ] Create executive summary
- [ ] Create navigation index
- [ ] Create completion status
- [ ] Add AI instructions file
- [ ] Update TASK-INDEX.md
- [ ] Link/copy files as appropriate
- [ ] Archive when complete

---

## 🔗 Cross-Reference Pattern

In all task files, include:

```markdown
## Related Documentation

- **Quick Start**: [README-<TASK>-<TYPE>.md](README-<TASK>-<TYPE>.md)
- **Detailed Guide**: [<TASK>-GUIDE.md](<TASK>-GUIDE.md)
- **Master Reference**: [REFACTORING-MAPPING.md](REFACTORING-MAPPING.md)
- **Navigation**: [INDEX-<TASK>.md](INDEX-<TASK>.md)
```

---

## 📞 Support

### Questions About Task Organization?

Refer to:
1. This file (TASK-ORGANIZATION-GUIDE.md)
2. The LiteBus migration as an example
3. TASK-INDEX.md for overview
4. AI-INSTRUCTIONS-MIGRATIONS.md for best practices

---

## 📈 Version History

| Version | Date           | Changes                              |
| ------- | -------------- | ------------------------------------ |
| 1.0     | October 30, 25 | Initial task organization guide      |

---

**Status**: ✅ Ready for Implementation  
**Purpose**: Guide for organizing future migrations and tasks  
**Template**: Use this for all new task documentation  

---
