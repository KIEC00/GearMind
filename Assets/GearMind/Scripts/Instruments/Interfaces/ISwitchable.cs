public interface ISwitchable
{
    public bool IsActive { get; }
    public void SetActive(bool isActive);
    public void Activate() => SetActive(true);
    public void Deactivate() => SetActive(false);
}
