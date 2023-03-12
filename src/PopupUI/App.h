#pragma once
#include "App.g.h"
#include "App.base.h"

namespace winrt::PopupUI::implementation
{
    struct App : AppT2<App>
    {
        App();
    };
}

namespace winrt::PopupUI::factory_implementation
{
    struct App : AppT<App, implementation::App>
    {
    };
}
