![Detached Banner](https://raw.githubusercontent.com/leonardoporro/Detached-Mapper/refs/heads/main/banner.png?raw=true)

> [!IMPORTANT]
> **Detached Mapper is in maintenance mode.**
> 10.0.0 supports EF Core 10. One final release, 11.0.0, will follow when EF Core 11 ships (November 2026); after that the repository will be archived.
>
> Its model lives on in [NSail.Mapping](https://github.com/nsail-ar/nsail-stack/blob/main/docs/mapping.md), part of the [NSail Stack](https://github.com/nsail-ar/nsail-stack): the same graph semantics, rebuilt as a compile-time source generator. No reflection, AOT-compatible, and build errors where Detached threw at runtime. It is not a drop-in replacement: it targets the NSail Stack (EF Core on PostgreSQL).


#### Maps Dtos directly to entities already attached to a DbContext
#### Solves disconnected entities, partial updates and automapping in a single step

## Getting started
Check [getting started](https://github.com/leonardoporro/Detached-Mapper/wiki/Getting-Started) on the new wiki.

## Play in .Net Fiddle
A couple of examples to check library capabilities without installing anything.

- [Attach graph of anonymous objects](https://dotnetfiddle.net/l7q6wA) 
- [Import json graph](https://dotnetfiddle.net/Mn8mYU)

## Issues and feedback
Please use [GitHub Issues](https://github.com/leonardoporro/Detached-Mapper/issues) tab to report anything you find using the library.
All feedback is welcome, I'll try to prioritize and fix everything, but I have limited personal time to work on this.

## Changes & Breaking Changes
Please take a look at the new list [here](https://github.com/leonardoporro/Detached-Mapper/wiki/Changes-&-Breaking-Changes).

## PR your test!
Fork the project and send me a PR with your tests in the Contrib folder and I will include them so that
in the future I won´t break your solution with changes. And I'll make it pass if they are broken because of an issue.
I know the library needs ton more of tests, but resources here are limited. Sorry and thanks for your contribution!

## Urgent issues
If something is blocking your project and you need help asap, contact me at my [e-mail](mailto:mail@leonardoporro.com.ar).

## Donations & Support

[![BuyCoffeeBanner](buy_coffee.png)](https://bmc.link/lporro)

If you like my work and want to [Buy me a Coffee](https://bmc.link/lporro), that would be awesome! ([mail me](mailto:mail@leonardoporro.com.ar) if that doesn't work for you)

Thanks for your feedback and kindly donations!:
- @RobertInDotNET and the entire team of SysTec Computer GmbH (https://www.systec-computer.de/)
- @traiannemes
- @MichaelM223
- @heyadamhey

And thanks to all the people who gave it a try and reported issues! Thanks for your help and patience.

## Related tools
Other packages built to provide features to the mapper:

- [Runtime Types](https://github.com/leonardoporro/Detached-RuntimeTypes)
- [Patch Types](https://github.com/leonardoporro/Detached-PatchTypes)
