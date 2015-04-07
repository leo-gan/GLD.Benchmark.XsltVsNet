The Xml transformation is an important part of system integration. The Xml documents are everywhere, despite surging JSON.

When we need to transform [or map] one Xml document to another Xml, we have several options. Two of them prevail. The first is the Xslt language. The second is the object transformation.

## Xslt Transformations ##
The [Xslt language](http://www.w3.org/TR/xslt) was created exactly for this purpose, to transform one Xml document to another Xml document. I am copying the Abstract of the Xslt standard here:  
*“This specification defines the syntax and semantics of XSLT, which is a language for transforming XML documents into other XML documents...”*

In reality to make the Xslt map we possibly need the [XML Schema](http://www.w3.org/TR/xmlschema11-1/) for source and target Xml documents. The XML Schemas are not mandatory but many Xslt editors use them to create Xslt maps. The BizTalk Server Mapper is such example.

The Xslt map is defined as the Xml document itself. The Xslt operators and expressions defined with help of the [XPath](http://www.w3.org/TR/xpath/), another Xml related standard.

## Object Transformations ##
An Object transformation is a transformation, when Xml is converted to the object graph of any programming language like Java, or C#. Then objects mapped to other objects, which converted back to Xml. Those two conversions could be performed by XmlSerializer which is part of .NET. The mapping written and executed as the generic C# code.

The Object transformation is not an official term. Usually in development you see terms “mapping”, “transformation”, “converting”.

## Comparison ##
Theoretically, the more specialized tools should always beat the less specialized. The Xslt designed for exactly this purpose, so the question is how better is it? Transformation speed is the most important feature, so I tested the speed.

# Test Project #
Tests have two parameters: the number of repetitions and the number of the nested objects.

The **repetitions** should stabilize the test measurements, make them statistically more correct. **Number of the nested objects** implicitly defines the size of the transformed Xml document.

## Test Data ##
The test Xml document is created by XmlSerializer from the Person class.

## Transformers ##
There are two transformers:

- XsltTransformer
- NetTransformer

The XsltTransformer uses the PersonToEmployee.xsl and PersonToPerson.xsl Xslt stylesheets. If you want to use the third-party mappers to create or change the stylesheets, I have generated Xml schemas for you; they are in the XmlSchema.xsd file.

NetTransformer uses the same XmlSerializer. The transformation code is simple and boring, nothing to say about it.

The transformers do not try to produce the same transformations. Small differences in transformations don’t matter for our case.

## Transformation and Enrichment ##
I have chosen two transformation types:

- **Enrichment**, when the source and target Xml documents have the same schemas. It used to change content of the documents without changing the document structure.
- **Transformation**, when the source and target Xml documents have different schemas. It creates a new target document using data of the source document. Target document has a schema, different from the source document schema.
For enrichment we operate with the same document structure, so theoretically enrichment should be simpler and faster.

## How We Test ##
The test document created and tested for Xslt and Object transformations, and for both Enrichment and Transformation types. It is one test cycle.

A new test document created for each test cycle. This eliminates the possibility of the optimizations, which could be performed on the OS, the memory management level. The data for the test document created randomly by Randomizer class.

The test classes are initialized on the first cycle, so it takes much more time to perform the first test. I measure the **maximum time**, because it is important for the real life situation, when we need only one transformation.

To measure the **average time** the 5% of maximum and 5% of minimum values are removed from calculation.

I also measure the **size** of the transformed Xml document. It shows the importance of the spaces and new line symbols for the document size.

## Note about Result Xml document ##

The sizes of the result Xml documents for Xslt and Object transformations should not be very different. Yes, you read me right. The result Xml documents should not be the same in each symbol for both transformations, and still documents can be recognized as equal. It is because of the ambiguity of the Xml standard. For example, the namespace prefixes could be different for the same namespace. In one result we can get “ns0:” prefix and the “abc12:” prefix in another, but both resolve the same namespace. As result the Xml documents got different size, but both are equal in terms of data values and structure. As result of this, we could not compare the Xml documents as the strings. We could converted Xml documents to the objects graph and compared the result object set. If all objects are equal, Xml documents are equal. I decided do not compare results because it is not the test goal. I just output the target Xml documents, so they could be easily compared, if needed.

[The last test result see on my blog](http://geekswithblogs.net/LeonidGaneline/archive/2015/03/28/xml-transformation-xslt-vs-.net.aspx). Please, don't trust the results! I did not spent time to optimize the code.
