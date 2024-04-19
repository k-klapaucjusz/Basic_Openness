﻿using Basic_Openness;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_Openness
{
    class KolekcjaZadan : ObservableCollection<Zadanie>
    {
        public KolekcjaZadan()
        {
            Add(new Zadanie
            {
                Nazwa = "Zamówienie",
                Opis = "Zamówić 100 długopisów żelowych",
                Priorytet = 1
            });
            Add(new Zadanie
            {
                Nazwa = "Zaproszenie",
                Opis = "Zaprosić kontrahentów na pokaz nowego produktu",
                Priorytet = 2
            });
            Add(new Zadanie
            {
                Nazwa = "Sprzątanie",
                Opis = "Posprzątać magazyn",
                Priorytet = 3
            });
        }
    }
}
