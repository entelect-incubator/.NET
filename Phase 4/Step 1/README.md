<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 4 - Step 1**

<br/><br/>

Standards

## **Install StyleCop**

[Overview](https://github.com/StyleCop/StyleCop)

Add Phase 4/Data/StyleCopRuleSet.ruleset to your Solution

![StyleCopRuleSet](2021-01-15-10-25-17.png)

Install StyleCop.Analyzers Nuget Package to all Projects

![StyleCop.Analyzers](2021-01-15-10-26-37.png)

Add the StyleCop Rule Set in every project file.

![](2021-01-15-10-30-33.png)

```
<CodeAnalysisRuleSet>$(SolutionDir)\StyleCopRuleSet.ruleset</CodeAnalysisRuleSet>
```

Fix build errors

Move to Step 2
[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%204/Step%202) 