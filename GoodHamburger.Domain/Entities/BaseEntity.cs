namespace GoodHamburger.Domain.Entities;

public abstract class BaseEntity
{
    public long Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public BaseEntity SetCreatedAt(DateTime date)
    {
        CreatedAt = date;
        UpdatedAt = date;
        return this;
    }

    public BaseEntity SetUpdatedAt(DateTime date){
        UpdatedAt = date;
        return this;
    }

    public BaseEntity SetDeletedAt(DateTime date){
        DeletedAt = date;
        return this;
    }
}
