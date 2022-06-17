using Bachelorprosjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePDF
{
    //This is not finished
    //TODO: change implementation from void to File?
    //TODO - either delete files in the end, or only store in memory
    //Målet er å returnere riktig pdf - utan at det ligg igjen pdf-filer etterpå.
    interface IPDFGenerator
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public byte[]? GenerateProsjektbeskrivelse(ProsjektDescription prosjektDescription, string filename);

        [MethodImpl(MethodImplOptions.Synchronized)]
        public byte[]? GenerateProsjektKatalog(List<ProsjektDescription> prosjektDescriptions, string navn);
        
        
    }
}
