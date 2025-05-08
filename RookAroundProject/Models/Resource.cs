namespace RookAroundProject;

public class Resource{
    public int Id { get; set; }
    public ResourceName Name { get; }
    public int Amount { get; set;}
    public bool IsPerishable { get; }

    protected Resource(){}
    public Resource(ResourceName name, int amount, bool isPerishable = false){
        Name = name;
        Amount = amount;
        IsPerishable = isPerishable;
    }
}
