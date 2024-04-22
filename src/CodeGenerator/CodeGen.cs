using CppSharp;
using CppSharp.Generators;
using CppSharp.Generators.Cpp;
using CppSharp.Parser;
using CppSharp.Passes;
using ASTContext = CppSharp.AST.ASTContext;

namespace CodeGenerator;

public class CodeGen : ILibrary
{
    private const string LibraryName = "nfd";
    
    public void Setup(Driver driver)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "vendor", "nativefiledialog-extended", "src", "include");

        driver.ParserOptions.Verbose = true;
        driver.ParserOptions.LanguageVersion = LanguageVersion.CPP17;

        var options = driver.Options;
        options.GenerateDefaultValuesForArguments = true;
        options.GeneratorKind = GeneratorKind.CSharp;
        options.GenerateSequentialLayout = true;
        options.GenerateFinalizers = true;
        options.Verbose = true;
        
        

        var module = options.AddModule(LibraryName);
        module.OutputNamespace = "NativeFileDialogNET";
        module.Defines.Add("BUILD_SHARED_LIBS=ON");
        module.Defines.Add("NFD_INSTALL=OFF");
        module.Defines.Add("NFD_BUILD_TESTS=OF");
        
        module.Headers.Add(Path.Combine(path, "nfd.h"));
    }

    public void SetupPasses(Driver driver)
    {
        // TODO Add More passes to improve generated code even more
        driver.Context.TranslationUnitPasses.RenameDeclsUpperCase(RenameTargets.Property);
        driver.Context.TranslationUnitPasses.RenameDeclsUpperCase(RenameTargets.Class);
        driver.Context.TranslationUnitPasses.AddPass(new CheckDuplicatedNamesPass());
        driver.Context.TranslationUnitPasses.AddPass(new CheckFlagEnumsPass());
        driver.Context.TranslationUnitPasses.AddPass(new FixDefaultParamValuesOfOverridesPass());
        driver.Context.TranslationUnitPasses.AddPass(new HandleDefaultParamValuesPass());
        driver.Context.TranslationUnitPasses.AddPass(new MakeProtectedNestedTypesPublicPass());
        driver.Context.TranslationUnitPasses.AddPass(new ConstructorToConversionOperatorPass());
        driver.Context.TranslationUnitPasses.AddPass(new MoveFunctionToClassPass());
        driver.Context.TranslationUnitPasses.AddPass(new CheckMacroPass());
        driver.Context.TranslationUnitPasses.AddPass(new FixupPureMethodsPass());
        driver.Context.TranslationUnitPasses.AddPass(new DelegatesPass());
        driver.Context.TranslationUnitPasses.AddPass(new GenerateSymbolsPass());
        driver.Context.TranslationUnitPasses.AddPass(new FunctionToStaticMethodPass());
        //driver.Context.TranslationUnitPasses.AddPass(new MakeInternalAndExternPass());
        //driver.Context.GeneratorOutputPasses.AddPass(new RenameOutputClasses());
    }
    
    public void Preprocess(Driver driver, ASTContext ctx)
    {
        // TODO
    }

    public void Postprocess(Driver driver, ASTContext ctx)
    {
        // TODO
    }
}