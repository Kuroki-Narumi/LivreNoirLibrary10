# BMSデータ構造
```
class BmsDataUnit
{
	HeaderCollection Headers;
	DefListCollection DefLists;
	BarDefCollection BarDefs;
	Timeline Timeline;
	List<Flow> Flows;
}
```

```
class BmsData
{
	ChartType ChartType;
	int LnObj;
	BmsDataUnit Root;
	List<BmsDataUnit> BrancheData;
}
```