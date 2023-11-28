def citeste_reguli(filename):
    with open(filename, 'r') as file:
        reguli = [linie.strip() for linie in file.readlines() if linie.strip() != '']
    return reguli


def extrage_neterminale(reguli):
    neterminale = set()
    for regula in reguli:
        cap, _ = regula.split('->')
        neterminale.add(cap.strip())
    return neterminale


def extrage_terminale(reguli, neterminale):
    terminale = set()
    for regula in reguli:
        for simbol in regula.split('->')[1].split():
            if simbol not in neterminale and simbol != 'eps':
                terminale.add(simbol)
    return terminale


def extrage_simbol_start(reguli):
    return reguli[0].split('->')[0].strip()


def extrage_reguli_de_productie(reguli):
    reguli_de_productie = []
    for regula in reguli:
        cap, corp = regula.split('->')
        reguli_de_productie.append((cap.strip(), [item.strip() for item in corp.split()]))
    return reguli_de_productie


# Functie pentru afisarea informatiilor despre gramatica
def afiseaza_informatii_gramatica(filename):
    reguli = citeste_reguli(filename)
    neterminale = extrage_neterminale(reguli)
    terminale = extrage_terminale(reguli, neterminale)
    simbol_start = extrage_simbol_start(reguli)
    reguli_de_productie = extrage_reguli_de_productie(reguli)

    print("Simbol de start:", simbol_start)
    print("Neterminale:", ', '.join(neterminale))
    print("Terminale:", ', '.join(terminale))
    print("Reguli de productie:")
    for neterminal, productie in reguli_de_productie:
        prod = ''.join(str(elem) for elem in productie)
        print(f"{neterminal} -> {prod}")


# Exemplu de utilizare
nume_fisier = 'input2.txt'
afiseaza_informatii_gramatica(nume_fisier)
