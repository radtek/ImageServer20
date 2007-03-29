require 'model'
require 'template'

# Main class for program execution
class CodeGen

  # specifies a set of templates that will be applied to entity classes
  @@entityTemplates = [
    Template.new("Entity.ct", "<%=@className%>.cs", false),
    Template.new("Entity.gen.ct", "<%=@className%>.gen.cs", true),
    Template.new("SearchCriteria.gen.ct", "<%=@className%>SearchCriteria.gen.cs", true),
    Template.new("IEntityBroker.gen.ct", "Brokers/I<%=@className%>Broker.gen.cs", true),
    Template.new("EntityBroker.gen.ct", "Hibernate/Brokers/<%=@className%>Broker.gen.cs", true)
    #Template.new("EntityInfo.gen.ct", "Support/<%=@className%>Info.gen.cs", true)
  ]
  
  # specifies a set of templates that will be applied to enum classes
  @@enumTemplates = [
    Template.new("enum.ct", "<%=@enumName%>.cs", false),
    Template.new("EnumValue.gen.ct", "<%=@className%>.gen.cs", true),
    Template.new("EnumTable.gen.ct", "<%=@className%>Table.gen.cs", true),
    Template.new("IEnumBroker.gen.ct", "Brokers/I<%=@className%>Broker.gen.cs", true),
    Template.new("EnumBroker.gen.ct", "Hibernate/Brokers/<%=@className%>Broker.gen.cs", true),
    Template.new("EnumHbm.gen.ct", "Hibernate/<%=@className%>Hbm.gen.cs", true)
  ]
  
    # specifies a set of templates that will be applied to component classes
  @@componentTemplates = [
    Template.new("Component.ct", "<%=@className%>.cs", false),
    Template.new("Component.gen.ct", "<%=@className%>.gen.cs", true),
    Template.new("SearchCriteria.gen.ct", "<%=@className%>SearchCriteria.gen.cs", true),
  ]
  
  @@queryTemplates = [
    Template.new("IQueryBroker.gen.ct", "Brokers/I<%=queryName%>Broker.gen.cs", true),
    Template.new("QueryBroker.gen.ct", "Hibernate/Brokers/<%=queryName%>Broker.gen.cs", true)
  ]

  @@queryResultTemplates = [
    Template.new("QueryResult.gen.ct", "<%=@className%>.gen.cs", true)
  ]
  
  @@queryCriteriaTemplates = [
    Template.new("QueryCriteria.gen.ct", "<%=@className%>.gen.cs", true)
  ]
  
  
  
  # total number of generated files
  @@count = 0

  # Runs the code generator on the specified srcDir and destDir
  # srcDir is a directory that contains any number of hibernate mapping (*.hbm.xml) files
  # destDir is the root directory where the generated code files should be placed
  def CodeGen.generate(srcDir, destDir)
    srcDir = File.expand_path(srcDir)
    destDir = File.expand_path(destDir)
    
    #check for bad input
    exitWithMessage("usage: inputDir outputDir") if ( srcDir == nil || destDir == nil )
    exitWithMessage("#{srcDir} does not exist") if !File.exist?(srcDir)
    exitWithMessage("#{destDir} does not exist") if !File.exist?(destDir)
    
    #create a new model object
    model = Model.new
    
    #enumerate hbm files in srcDir to build model
    Dir.entries(srcDir).each do |file|
    
      #check if the filename matches the *.hbm.xml extension
      if File.fnmatch?("*.hbm.xml", file, File::FNM_DOTMATCH) || File.fnmatch?("*.hrq.xml", file, File::FNM_DOTMATCH)
        #add the file to the model
    	filePath = File.expand_path(file, srcDir)
#    	if(!shouldIgnoreFile(filePath))
    		model.add(filePath, shouldIgnoreFile(filePath))
#    	end
      end
    end
    
    #process each template
    applyTemplates(@@entityTemplates, model.entityDefs, destDir)
    applyTemplates(@@enumTemplates, model.enumDefs, destDir)
    applyTemplates(@@componentTemplates, model.componentDefs, destDir)
    applyTemplates(@@queryTemplates, model.queryDefs, destDir)
    
    # note that we process the criteria/result classes for each QueryDef
    # inherited criteria/result classes are ignored because it would be redundant to generate them twice (hence the call to Array::compact)
    applyTemplates(@@queryResultTemplates, model.queryDefs.map {|queryDef| queryDef.resultClass(false) }.compact, destDir)
    applyTemplates(@@queryCriteriaTemplates, model.queryDefs.map {|queryDef| queryDef.criteriaClass(false) }.compact, destDir)
    
    
    puts "Total #{@@count} files"
  end
  
  # applies the specified array of templates to the specified array of elementDefs, placing output into destDir
  def CodeGen.applyTemplates(templates, elementDefs, destDir)
    elementDefs.each do |elementDef|
      if(elementDef.suppressCodeGen)
        puts "Skipping " + elementDef.elementName
      else
        puts "Processing " + elementDef.elementName
        templates.each do |template|
          outputFile = template.run(elementDef, "./templates", destDir)
          if (outputFile) 
            @@count += 1
            puts "Generated " + outputFile.sub(destDir, "")
          end
        end
      end
    end
  end
  
  # prints the specified message and exits
  def CodeGen.exitWithMessage(msg)
    puts msg
    exit
  end
  
  def CodeGen.shouldIgnoreFile(filename)
	  ignore = nil
	  open(filename) do |file|
		  ignore = file.readlines.detect { |line| line.include?("<!--") && line.include?("@codegen") && line.include?("ignore") }
	  end
	  return ignore != nil
  end
end

# execute program using command line args
CodeGen.generate(*ARGV)





