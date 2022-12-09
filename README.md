# SDD_ASG2

## Frequently used links

<kbd> <br> [Trello][Trello] <br> </kbd> <kbd> <br> [Database][Database] <br> </kbd> <kbd> <br> [Bootstrap][Bootstrap] <br> </kbd> <kbd> <br> [Website][Website] <br> </kbd> <br>

[Trello]: https://trello.com/b/puyM2gaE/asignment-2 'Task Board'
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
## SOP
<details>
<summary>More info</summary>

### GIT 
1. Create Branch for each new feature
2. Join Branch to master once feature is stable and completed 

## DAL classes
> Name: classDAL.cs

</details>

## Configure MySQL LocalDB
<details>
<summary>More info</summary>

> Credentials are found in Whatsapp group description

## Create Database
1. Download [XAMPP][XAMPP]
2. Run XAMPP as administrator
3. Click on the check box for Apache and MySQL an start them (will need to start them every time you want to use)
4. Edit Database (Select any)
    - Click on Admin in MySQL row to edit database (XAMPP)
    - Download [DataGrip][DataGrip] (JetBrain offer free education license to student | [Register Here][JetBrain Register])
    - Download this crack app [dbForge Studio for MySQL][dbForge Studio for MySQL]
5. Run this [MySQL script][MySQL script] in your MySQL IDE 

## Configure Database IDE

### DataGrip

### dbForge Studio for MySQL




[XAMPP]: https://www.apachefriends.org/download.html 'Download'
[DataGrip]: https://www.jetbrains.com/datagrip/ 'Download'
[JetBrain Register]: https://www.jetbrains.com/community/education/#students 'Register'
[dbForge Studio for MySQL]: https://qwldr-my.sharepoint.com/:f:/g/personal/cheaposoftware_qwldr_onmicrosoft_com/EkR6n4Znn61Ilo2y_Wy2TtoBnWCfKLvXN_o_6JBYsa8vKw?e=rFr6df
[MySQL script]: https://github.com/nathenlow/SDD_ASG2/raw/master/setup_ngeeanncity.sql 'Setup database'
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
