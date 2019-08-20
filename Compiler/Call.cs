using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using static Compiler.LiteParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class LiteLangVisitor{
public  override  object VisitCallExpression( CallExpressionContext context ){
var r = ((Result)(Visit(context.id())));
r.text="."+r.text;
if ( context.templateCall()!=null ) {
r.text+=((string)(Visit(context.templateCall())));
}
if ( context.callFunc()!=null ) {
var e2 = ((Result)(Visit(context.callFunc())));
r.text=r.text+e2.text;
}
else if ( context.callElement()!=null ) {
var e2 = ((Result)(Visit(context.callElement())));
r.text=r.text+e2.text;
}
else if ( context.callChannel()!=null ) {
var e2 = ((Result)(Visit(context.callChannel())));
r.text=r.text+e2.text;
}
return r;
}
public  override  object VisitCallElement( CallElementContext context ){
if ( context.expression()==null ) {
return ((new Result(){text = ((string)(Visit(context.slice())))}));
}
var r = ((Result)(Visit(context.expression())));
r.text=(new System.Text.StringBuilder("[").Append(r.text).Append("]")).to_str();
return r;
}
public  override  object VisitSlice( SliceContext context ){
return ((string)(Visit(context.GetChild(0))));
}
public  override  object VisitSliceFull( SliceFullContext context ){
var order = "";
var attach = "";
switch (context.op.Text) {
case "<=" :
{ order = "true";
attach = "true";
}break;
case "<" :
{ order = "true";
}break;
case ">=" :
{ order = "false";
attach = "true";
}break;
case ">" :
{ order = "false";
}break;
}
var expr1 = ((Result)(Visit(context.expression(0))));
var expr2 = ((Result)(Visit(context.expression(1))));
return (new System.Text.StringBuilder(".slice(").Append(expr1.text).Append(", ").Append(expr2.text).Append(", ").Append(order).Append(", ").Append(attach).Append(")")).to_str();
}
public  override  object VisitSliceStart( SliceStartContext context ){
var order = "";
var attach = "";
switch (context.op.Text) {
case "<=" :
{ order = "true";
attach = "true";
}break;
case "<" :
{ order = "true";
}break;
case ">=" :
{ order = "false";
attach = "true";
}break;
case ">" :
{ order = "false";
}break;
}
var expr = ((Result)(Visit(context.expression())));
return (new System.Text.StringBuilder(".slice(").Append(expr.text).Append(", null, ").Append(order).Append(", ").Append(attach).Append(")")).to_str();
}
public  override  object VisitSliceEnd( SliceEndContext context ){
var order = "";
var attach = "false";
switch (context.op.Text) {
case "<=" :
{ order = "true";
attach = "true";
}break;
case "<" :
{ order = "true";
}break;
case ">=" :
{ order = "false";
attach = "true";
}break;
case ">" :
{ order = "false";
}break;
}
var expr = ((Result)(Visit(context.expression())));
return (new System.Text.StringBuilder(".slice(null, ").Append(expr.text).Append(", ").Append(order).Append(", ").Append(attach).Append(")")).to_str();
}
public  override  object VisitCallFunc( CallFuncContext context ){
var r = (new Result(){data = "var"});
if ( context.tuple()!=null ) {
r.text+=((Result)(Visit(context.tuple()))).text;
}
else {
r.text+=(new System.Text.StringBuilder("(").Append(((Result)(Visit(context.lambda()))).text).Append(")")).to_str();
}
return r;
}
public  override  object VisitCallPkg( CallPkgContext context ){
var r = (new Result(){data = Visit(context.typeType())});
r.text=(new System.Text.StringBuilder("(new ").Append(Visit(context.typeType())).Append("()")).to_str();
if ( context.pkgAssign()!=null ) {
r.text+=Visit(context.pkgAssign());
}
else if ( context.listAssign()!=null ) {
r.text+=Visit(context.listAssign());
}
else if ( context.setAssign()!=null ) {
r.text+=Visit(context.setAssign());
}
else if ( context.dictionaryAssign()!=null ) {
r.text+=Visit(context.dictionaryAssign());
}
r.text+=")";
return r;
}
public  override  object VisitCallNew( CallNewContext context ){
var r = (new Result(){data = Visit(context.typeType())});
var param = "";
if ( context.expressionList()!=null ) {
param = ((Result)(Visit(context.expressionList()))).text;
}
r.text=(new System.Text.StringBuilder("(new ").Append(Visit(context.typeType())).Append("(").Append(param).Append(")")).to_str();
r.text+=")";
return r;
}
public  override  object VisitPkgAssign( PkgAssignContext context ){
var obj = "";
obj+="{";
foreach (var i in range(0,context.pkgAssignElement().Length,1,true,false)){
if ( i==0 ) {
obj+=Visit(context.pkgAssignElement(i));
}
else {
obj+=","+Visit(context.pkgAssignElement(i));
}
}
obj+="}";
return obj;
}
public  override  object VisitListAssign( ListAssignContext context ){
var obj = "";
obj+="{";
foreach (var i in range(0,context.expression().Length,1,true,false)){
var r = ((Result)(Visit(context.expression(i))));
if ( i==0 ) {
obj+=r.text;
}
else {
obj+=","+r.text;
}
}
obj+="}";
return obj;
}
public  override  object VisitSetAssign( SetAssignContext context ){
var obj = "";
obj+="{";
foreach (var i in range(0,context.expression().Length,1,true,false)){
var r = ((Result)(Visit(context.expression(i))));
if ( i==0 ) {
obj+=r.text;
}
else {
obj+=","+r.text;
}
}
obj+="}";
return obj;
}
public  override  object VisitDictionaryAssign( DictionaryAssignContext context ){
var obj = "";
obj+="{";
foreach (var i in range(0,context.dictionaryElement().Length,1,true,false)){
var r = ((DicEle)(Visit(context.dictionaryElement(i))));
if ( i==0 ) {
obj+=r.text;
}
else {
obj+=","+r.text;
}
}
obj+="}";
return obj;
}
public  override  object VisitPkgAssignElement( PkgAssignElementContext context ){
var obj = "";
obj+=Visit(context.name())+" = "+((Result)(Visit(context.expression()))).text;
return obj;
}
public  override  object VisitPkgAnonymous( PkgAnonymousContext context ){
return (new Result(){data = "var",text = "new"+((string)(Visit(context.pkgAnonymousAssign())))});
}
public  override  object VisitPkgAnonymousAssign( PkgAnonymousAssignContext context ){
var obj = "";
obj+="{";
foreach (var i in range(0,context.pkgAnonymousAssignElement().Length,1,true,false)){
if ( i==0 ) {
obj+=Visit(context.pkgAnonymousAssignElement(i));
}
else {
obj+=","+Visit(context.pkgAnonymousAssignElement(i));
}
}
obj+="}";
return obj;
}
public  override  object VisitPkgAnonymousAssignElement( PkgAnonymousAssignElementContext context ){
var obj = "";
obj+=Visit(context.name())+" = "+((Result)(Visit(context.expression()))).text;
return obj;
}
public  override  object VisitCallAwait( CallAwaitContext context ){
var r = (new Result());
var expr = ((Result)(Visit(context.expression())));
r.data="var";
r.text="await "+expr.text;
return r;
}
public  override  object VisitList( ListContext context ){
var type = Any;
var result = (new Result());
foreach (var i in range(0,context.expression().Length,1,true,false)){
var r = ((Result)(Visit(context.expression(i))));
if ( i==0 ) {
type = ((string)(r.data));
result.text+=r.text;
}
else {
if ( type!=((string)(r.data)) ) {
type = Any;
}
result.text+=","+r.text;
}
}
result.data=(new System.Text.StringBuilder("").Append(Lst).Append("<").Append(type).Append(">")).to_str();
result.text=(new System.Text.StringBuilder("(new ").Append(result.data).Append("(){ ").Append(result.text).Append(" })")).to_str();
return result;
}
public  override  object VisitSet( SetContext context ){
var type = Any;
var result = (new Result());
foreach (var i in range(0,context.expression().Length,1,true,false)){
var r = ((Result)(Visit(context.expression(i))));
if ( i==0 ) {
type = ((string)(r.data));
result.text+=r.text;
}
else {
if ( type!=((string)(r.data)) ) {
type = Any;
}
result.text+=","+r.text;
}
}
result.data=(new System.Text.StringBuilder("").Append(Set).Append("<").Append(type).Append(">")).to_str();
result.text=(new System.Text.StringBuilder("(new ").Append(result.data).Append("(){ ").Append(result.text).Append(" })")).to_str();
return result;
}
public  override  object VisitDictionary( DictionaryContext context ){
var key = Any;
var value = Any;
var result = (new Result());
foreach (var i in range(0,context.dictionaryElement().Length,1,true,false)){
var r = ((DicEle)(Visit(context.dictionaryElement(i))));
if ( i==0 ) {
key = r.key;
value = r.value;
result.text+=r.text;
}
else {
if ( key!=r.key ) {
key = Any;
}
if ( value!=r.value ) {
value = Any;
}
result.text+=","+r.text;
}
}
var type = key+","+value;
result.data=(new System.Text.StringBuilder("").Append(Dic).Append("<").Append(type).Append(">")).to_str();
result.text=(new System.Text.StringBuilder("(new ").Append(result.data).Append("(){ ").Append(result.text).Append(" })")).to_str();
return result;
}
public  override  object VisitDictionaryElement( DictionaryElementContext context ){
var r1 = ((Result)(Visit(context.expression(0))));
var r2 = ((Result)(Visit(context.expression(1))));
var result = (new DicEle(){key = ((string)(r1.data)),value = ((string)(r2.data)),text = "{"+r1.text+","+r2.text+"}"});
return result;
}
public  override  object VisitFunctionExpression( FunctionExpressionContext context ){
var r = (new Result());
if ( context.t.Type==Right_Flow ) {
r.text+=" async ";
}
r.text+=Visit(context.parameterClauseIn())+" => "+BlockLeft+Wrap;
this.add_current_set();
r.text+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
r.text+=BlockRight+Wrap;
r.data="var";
return r;
}
public  override  object VisitLambda( LambdaContext context ){
var r = (new Result(){data = "var"});
if ( context.t.Type==Right_Flow ) {
r.text+="async ";
}
r.text+="(";
if ( context.lambdaIn()!=null ) {
r.text+=Visit(context.lambdaIn());
}
r.text+=")";
r.text+="=>";
if ( context.tupleExpression()!=null ) {
r.text+=((Result)(Visit(context.tupleExpression()))).text;
}
else {
this.add_current_set();
r.text+="{"+ProcessFunctionSupport(context.functionSupportStatement())+"}";
this.delete_current_set();
}
return r;
}
public  override  object VisitLambdaIn( LambdaInContext context ){
var obj = "";
foreach (var i in range(0,context.id().Length,1,true,false)){
var r = ((Result)(Visit(context.id(i))));
if ( i==0 ) {
obj+=r.text;
}
else {
obj+=", "+r.text;
}
this.add_id(r.text);
}
return obj;
}
}
}
