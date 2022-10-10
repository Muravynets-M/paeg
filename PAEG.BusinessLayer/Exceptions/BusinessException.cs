namespace PAEG.BusinessLayer.Exceptions; 

public class BusinessException: Exception {
    public Guid Id { get; set; }
}