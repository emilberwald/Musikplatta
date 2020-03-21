#pragma once
#include "L1ApiWintab.h"

#include <map>
#include <string>
#include <tuple>

std::map<int, std::tuple<std::string, std::string>> explain_wt{
	{ WT_PACKET,
	  {
		  "WT_PACKET",
		  "The WT_PACKET message is posted to the context-owning windows that have re­quested "
		  "messag­ing for their context.",
	  } },
	{ WT_CSRCHANGE,
	  {
		  "WT_CSRCHANGE",
		  "The WT_CSRCHANGE message is posted to the owning window when a new cur­sor enters the "
		  "context.",
	  } },
	{ WT_CTXOPEN,
	  {
		  "WT_CTXOPEN",
		  "The WT_CTXOPEN message is sent to the owning window and to any manager windows when a "
		  "context is opened.",
	  } },
	{ WT_CTXCLOSE,
	  {
		  "WT_CTXCLOSE",
		  "The WT_CTXCLOSE message is sent to the owning win­dow and to any manager windows when a "
		  "context is about to be closed.",
	  } },
	{ WT_CTXUPDATE,
	  {
		  "WT_CTXUPDATE",
		  "The WT_CTXUPDATE message is sent to the owning window and to any manager windows when a "
		  "context is changed.",
	  } },
	{ WT_CTXOVERLAP,
	  {
		  "WT_CTXOVERLAP",
		  "The WT_CTXOVERLAP message is sent to the owning window and to any man­ager windows when "
		  "a context is moved in the overlap order.",
	  } },
	{ WT_PROXIMITY,
	  {
		  "WT_PROXIMITY",
		  "The WT_PROXIMITY message is posted to the owning window and any manager windows when "
		  "the cursor enters or leaves context prox­imity.",
	  } },
	{ WT_INFOCHANGE,
	  {
		  "WT_INFOCHANGE",
		  "The WT_INFOCHANGE message is sent to all man­ager and context-owning win­dows when the "
		  "number of connected tablets has changed",
	  } },
};