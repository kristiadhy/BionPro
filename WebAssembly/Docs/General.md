`Class
Most of all pages use partial class to separate between the UI and the logic. This is a good practice to keep the code clean and easy to maintain.
If that is the case, then be careful to not using private identifiers in the partial class. Because the private identifiers are not accessible in the other partial class!

```csharp