<?xml version="1.0" encoding="UTF-16"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:var="http://schemas.microsoft.com/BizTalk/2003/var" exclude-result-prefixes="msxsl var" version="1.0">
  <xsl:output omit-xml-declaration="yes" method="xml" version="1.0" />
  <xsl:template match="/">
    <xsl:apply-templates select="/Person" />
  </xsl:template>
  <xsl:template match="/Person">
    <Employee>
      <xsl:if test="FirstName">
        <FirstName>
          <xsl:value-of select="FirstName/text()" />
        </FirstName>
      </xsl:if>
      <xsl:if test="LastName">
        <LastName>
          <xsl:value-of select="LastName/text()" />
        </LastName>
      </xsl:if>
      <Age>
        <xsl:value-of select="Age/text()" />
      </Age>
      <Gender>
        <xsl:value-of select="Gender/text()" />
      </Gender>
      <xsl:for-each select="Passport">
        <Passport>
          <xsl:if test="Number">
            <Number>
              <xsl:value-of select="Number/text()" />
            </Number>
          </xsl:if>
          <xsl:if test="Authority">
            <Authority>
              <xsl:value-of select="Authority/text()" />
            </Authority>
          </xsl:if>
          <ExpirationDate>
            <xsl:value-of select="ExpirationDate/text()" />
          </ExpirationDate>
        </Passport>
      </xsl:for-each>
      <HistoryRecords>
        <xsl:for-each select="PoliceRecords">
          <xsl:for-each select="PoliceRecord">
            <HistoryRecord>
              <Id>
                <xsl:value-of select="Id/text()" />
              </Id>
            </HistoryRecord>
          </xsl:for-each>
        </xsl:for-each>
      </HistoryRecords>
    </Employee>
  </xsl:template>
</xsl:stylesheet>