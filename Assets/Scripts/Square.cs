using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    //TO DO: 1:1 relationships and n:1 relationships etc. implemantation in OOP
    //for example, square has 1 hero or 0 hero. and hero has always 1 square
    //solution: hero has field square, should be sufficient.
    //Square can also have hero field, which must be able to be null. But this field shouldn't be needed

    //CombatAttack has always 1 strike. Strike has 0 CombatAttacks, but many CombatAttacks may point at one strike
    //solution:  CombatAttack has field strike. Strike have no fields on combatattacks

    //combatattack belongs to 1 combatcombo, combatcombo contains 0..n combatattacks
    //solution: clearest way would be that both have fields of each other. this way it is easiest to search
    //but this needs a strict system to not allow flawed code. And it isn't DRY

    //etc. A general solution needs to be found
    //and go through all scripts to find all possible connections. (hero and piece, hero and team, etc.)
}
