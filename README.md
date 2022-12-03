# SDD_ASG2

## Frequently used links

<kbd> <br> [Trello][Trello] <br> </kbd> <kbd> <br> [Database][Database] <br> </kbd> <kbd> <br> [Bootstrap][Bootstrap] <br> </kbd> <kbd> <br> [Website][Website] <br> </kbd> <br>

[Trello]: https://trello.com/w/sddteame 'Task Board'
[Database]: https://ns54.netcfm.com/mydatabase/db_operations.php?server=1&db=ngeeanncity& 'phpMyAdmin'
[Bootstrap]: https://getbootstrap.com/docs/5.2/getting-started/introduction/ 'Get started with Bootstrap'
[Website]: http://ngeeanncity.ga/ 'Ngee Ann City'

## Basic 
### ViewData
```ruby
@{
    ViewData["Title"] = "ContactUs";
    ViewData["css"] = "home.css";
    ViewData["bodyid"] = "contactus"
}
```
### Javascripts
```ruby
@section scripts {
    <script type="text/javascript"></script>
}
```
## Naming Convention
<details>
<summary>More info</summary>



</details>

## Layouts

<details>
<summary>More info</summary>

### Banner Layout
> Grey background with ViewData["HeaderTitle"] under the Navbar
```ruby
@{
    Layout = "~/Views/Shared/Main/_Title.cshtml";
    ViewData["HeaderTitle"] = "Contact Us";    
    ViewData["Title"] = "ContactUs";
    ViewData["css"] = "home.css";
}

<section class= "body"> .... </section>
```

</details>

## Navbar

<details>
<summary>More info</summary>

### Create Navbar in Shared/Navbar
```ruby
<ul class="nk-nav">
    <li>
        <a href="~/Home"> Home </a>
    </li>
    <li>
        <a href="~/Home/Login"> Login </a>
    </li>
</ul>
```

### Add role for each Navbar in Shared/_Layout.cshtml
```ruby
switch (role)
{
    case "Sales Personnel":
        @await Html.PartialAsync("Navbar/_SalesPersonnelNav.cshtml")
        break;
    case "Marketing Personnel":
        @await Html.PartialAsync("Navbar/_MarketingManagerNav.cshtml")
        break;
    case "Product Manager":
        @await Html.PartialAsync("Navbar/_ProductManagerNav.cshtml")
        break;
    case "Member":
        @await Html.PartialAsync("Navbar/_MemberNav.cshtml")
        break;
    default:
        @await Html.PartialAsync("Navbar/_HomeNav.cshtml")
        break;
}
```
</details>
