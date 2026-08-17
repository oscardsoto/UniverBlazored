const sessions = new Map();

function getApi(instanceId) {
    if (instanceId && sessions.has(instanceId))
        return sessions.get(instanceId).univerAPI;
    return window.univerAPI;
}

function getSession(instanceId) {
    if (!sessions.has(instanceId)) {
        if (instanceId)
            sessions.set(instanceId, { univerAPI: null, listenerNET: null });
        else if (!window.univerAPI)
            return null;
        else
            return sessions.get(instanceId);
    }
    return sessions.get(instanceId);
}

export function initUniver(instanceId, config, language) {
    const { createUniver } = UniverPresets;
    const { LocaleType, merge } = UniverCore;
    const { defaultTheme } = UniverDesign;
    const languageConfig = getLanguageConfig(config, language, LocaleType, merge);

    const { univerAPI } = createUniver({
        locale: languageConfig.locale,
        locales: languageConfig.locales,
        theme: defaultTheme,
        presets: getPresets(config),
        plugins: getPlugins(config)
    });

    univerAPI.createUniverSheet({ name: config.newSheetName })

    const session = { univerAPI, listenerNET: null };
    sessions.set(instanceId, session);

    session.univerAPI.getUserService = function() { return session.univerAPI.getUserManager()._userManagerService }

    session.univerAPI.addEvent(session.univerAPI.Event.SheetEditEnded, async (e) => {
        var info = {
            row: e.row,
            col: e.column,
            unitId: e.workbook.id,
            subUnitId: e.worksheet.getSheetId(),
            instanceId: instanceId
        }
        if (!session.listenerNET || !session.listenerNET.listeners.includes(info))
            return

        var cellValue = e.worksheet.getRange(info.row, info.col).getValue()
        await session.listenerNET.net.invokeMethodAsync("OnDataChanged", info, cellValue)
    })
}

function getLanguageConfig(config, language, LocaleType, merge){
    const localeType = getLocaleType(language, LocaleType);
    return {
        locale: localeType,
        locales: {
            [localeType]: getLocaleResources(config, localeType, merge)
        }
    }
}

function getLocaleType(language, LocaleType){
    switch (language.value){
        case "ru-RU": return LocaleType.RU_RU;
        case "zh-CN": return LocaleType.ZH_CN;
        case "vi-VN": return LocaleType.VI_VN;
        case "fa-IR": return LocaleType.FA_IR;
        case "ja-JP": return LocaleType.JA_JP;
        case "ko-KR": return LocaleType.KO_KR;
        case "es-ES": return LocaleType.ES_ES;
        case "ca-ES": return LocaleType.CA_ES;
        case "en-US":
        default:
            return LocaleType.EN_US;
    }
}

function getLocaleResources(config, localeType, merge){
    const suffix = getLocaleSuffix(localeType, UniverCore.LocaleType);
    return merge(
        {},
        getLocaleGlobalValue(`UniverPresetSheetsCore${suffix}`),
        config.hasShort ? getLocaleGlobalValue(`UniverPresetSheetsSort${suffix}`) : null,
        config.hasDataValidation ? getLocaleGlobalValue(`UniverPresetSheetsDataValidation${suffix}`) : null,
        config.hasFilter ? getLocaleGlobalValue(`UniverPresetSheetsFilter${suffix}`) : null,
        config.hasConditionalFormatting ? getLocaleGlobalValue(`UniverPresetSheetsConditionalFormatting${suffix}`) : null,
        config.hasHyperLink ? getLocaleGlobalValue(`UniverPresetSheetsHyperLink${suffix}`) : null,
        config.hasThreadComment ? getLocaleGlobalValue(`UniverPresetSheetsThreadComment${suffix}`) : null,
        config.hasDrawing ? getLocaleGlobalValue(`UniverPresetSheetsDrawing${suffix}`) : null,
        null,
        null
    )
}

function getLocaleSuffix(localeType, LocaleType){
    switch (localeType){
        case LocaleType.RU_RU: return "RuRU"
        case LocaleType.ZH_CN: return "ZhCN"
        case LocaleType.VI_VN: return "ViVN"
        case LocaleType.FA_IR: return "FaIR"
        case LocaleType.JA_JP: return "JaJP"
        case LocaleType.KO_KR: return "KoKR"
        case LocaleType.ES_ES: return "EsES"
        case LocaleType.CA_ES: return "CaES"
        case LocaleType.EN_US:
        default:
            return "EnUS"
    }
}

function getLocaleGlobalValue(localeKey){
    return globalThis[localeKey]
}

function getPresets(config){
    var presets = []
    const { UniverSheetsCorePreset } = UniverPresetSheetsCore
    presets.push(UniverSheetsCorePreset({
        container: config.idDiv,
        customFontFamily: config.fontsConfig
    }))

    if (config.hasShort){
        const { UniverSheetsSortPreset } = UniverPresetSheetsSort
        presets.push(UniverSheetsSortPreset())
    }

    if (config.hasDataValidation){
        const { UniverSheetsDataValidationPreset } = UniverPresetSheetsDataValidation
        presets.push(UniverSheetsDataValidationPreset())
    }

    if (config.hasFilter){
        const { UniverSheetsFilterPreset } = UniverPresetSheetsFilter
        presets.push(UniverSheetsFilterPreset())
    }

    if (config.hasConditionalFormatting){
        const { UniverSheetsConditionalFormattingPreset } = UniverPresetSheetsConditionalFormatting
        presets.push(UniverSheetsConditionalFormattingPreset())
    }

    if (config.hasHyperLink){
        const { UniverSheetsHyperLinkPreset } = UniverPresetSheetsHyperLink
        presets.push(UniverSheetsHyperLinkPreset())
    }

    if (config.hasDrawing){
        const { UniverSheetsDrawingPreset } = UniverPresetSheetsDrawing
        presets.push(UniverSheetsDrawingPreset())
    }

    if (config.hasThreadComment){
        const { UniverSheetsThreadCommentPreset } = UniverPresetSheetsThreadComment
        presets.push(UniverSheetsThreadCommentPreset())
    }

    return presets
}

function getPlugins(config){
    var plugins = []
    if (config.hasCrosshair){
        const { UniverSheetsCrosshairHighlightPlugin } = UniverSheetsCrosshairHighlight
        plugins.push(UniverSheetsCrosshairHighlightPlugin)
    }

    if (config.hasWatermark){
        const { UniverWatermarkPlugin } = UniverWatermark
        plugins.push([UniverWatermarkPlugin,
        {
            textWatermarkSettings: {content: config.watermarkLabel}
        }])
    }

    return plugins
}

export function getAndExecuteMethod(instanceId, queue, toReturn){
    var api = getApi(instanceId)
    if (!api) return { res: false }

    var result = null
    for (var i = 0; i < queue.length; i++){
        if (i == 0){
            if (typeof api[queue[i].methodName] === "function"){
                if (queue[i].args.length == 0)
                    result = api[queue[i].methodName]()
                else
                    result = api[queue[i].methodName](...queue[i].args)
            }
            else{
                console.error(`Method ${i}: ${queue[i].methodName} does not exist on Univer API`);
                console.error(result)
                return { res:false }
            }
            continue;
        }

        if (typeof result[queue[i].methodName] === "function"){
            if (queue[i].args.length == 0)
                result = result[queue[i].methodName]()
            else
                result = result[queue[i].methodName](...queue[i].args)
        }
        else{
            console.error(`Method ${i}: ${queue[i].methodName} does not exist on Univer API`);
            console.error(result)
            return { res:false }
        }
    }
    if (toReturn)
        return { res:result }
    return { res:true }
}

function isPromise(value) {
    return value && (typeof value === 'object' || typeof value === 'function') && typeof value.then === 'function';
}

function resolveInstanceId(instanceId_or_snapshot, snapshot) {
    if (snapshot !== undefined) return instanceId_or_snapshot;
    if (instanceId_or_snapshot && instanceId_or_snapshot.instanceId) return instanceId_or_snapshot.instanceId;
    return null;
}

function selectSheet(instanceId, snapshot){
    var api = getApi(instanceId)
    if (!api) return null

    if (!snapshot.sheetSelected)
        throw new Error("Sheet context is required. Call OnSheet(sheet) before executing this command.")

    return api.getActiveWorkbook().getSheetBySheetId(snapshot.sheetSelected.id)
}

function selectRange(instanceId, snapshot){
    var sheet = selectSheet(instanceId, snapshot)
    if (!sheet) return null

    if (!snapshot.rangeSelected)
        throw new Error("Range context is required. Call OnRange(range) before executing this command.")

    return sheet.getRange(snapshot.rangeSelected)
}

export function areScriptsReady(){
    return typeof UniverPresets !== 'undefined'
        && typeof UniverCore !== 'undefined'
        && typeof UniverDesign !== 'undefined';
}

export function getSheetsInfo(instanceId){
    var api = getApi(instanceId)
    if (!api) return []

    var result = []
    api.getActiveWorkbook().getSheets().forEach(sheet => {
        result.push({
            id: sheet.getSheetId(),
            name: sheet.getSheetName(),
            maxUsed: sheet.getDataRange().getRange(),
            tabColor: sheet.getTabColor(),
            isHidden: sheet.isSheetHidden(),
            rowsHidden: sheet._worksheet.getRowManager().getHiddenRows(),
            columnsHidden: sheet._worksheet.getColumnManager().getHiddenCols()
        })
    })
    return result
}

export function hasFilter(snapshot){
    var instanceId = snapshot.instanceId
    var sheet = selectSheet(instanceId, snapshot)
    if (!sheet) return false

    var filter = sheet.getFilter()
    if (filter)
        return true
    return false
}

export function getCellsStylesInfo(snapshot){
    var instanceId = snapshot.instanceId
    var activeRange = selectRange(instanceId, snapshot)
    if (!activeRange) return []

    var mapStyles = []
    activeRange.getCellStyles().forEach(styleArray => {
        var arrayStyles = []
        styleArray.forEach(style => {
            if (!style){
                arrayStyles.push(null)
            }
            else {
                var val = style.getValue()
                if (Object.keys(val).length === 0){
                    arrayStyles.push(null)
                }
                else{
                    arrayStyles.push(val)
                }
            }
        })
        mapStyles.push(arrayStyles)
    })

    var dictionary = []
    var { startRow, startColumn, endRow, endColumn } = activeRange.getRange();
    for (var row = 0; row < mapStyles.length; row++){
        for (var col = 0; col < mapStyles[row].length; col++){
            if (!mapStyles[row][col])
                continue;

            var style = mapStyles[row][col]
            var condt = kv => _.isEqual(kv.s, style)
            if (dictionary.some(condt)){
                var maped = dictionary.find(condt)
                maped.p.push([startRow + row, startColumn + col])
            }
            else{
                dictionary.push({s: style, p:[[startRow + row, startColumn + col]]})
            }
        }
    }
    return dictionary
}

export function setRangeStyles(snapshot, style, ranges)
{
    var instanceId = snapshot.instanceId
    var activeSheet = selectSheet(instanceId, snapshot)
    if (!activeSheet) return

    ranges.forEach((range) => {
        var selectRange = activeSheet.getRange(range)
        if (style.color !== null)
            selectRange.setFontColor(style.color);

        if (style.family !== null)
            selectRange.setFontFamily(style.family);

        if (style.strikethrough !== null)
            selectRange.setFontLine(style.strikethrough ? "line-through" : "none");

        if (style.underline !== null)
            selectRange.setFontLine(style.underline ? "underline" : "none");

        if (style.italic !== null)
            selectRange.setFontStyle(style.italic ? "italic" : "normal");

        if (style.size !== null)
            selectRange.setFontSize(style.size);

        if (style.bold !== null)
            selectRange.setFontWeight(style.bold ? "bold" : "normal");

        if (style.horizontalAlign !== null){
            switch (style.horizontalAlign)
            {
                case 0:
                    selectRange.setHorizontalAlignment("left");
                    break;
                case 1:
                    selectRange.setHorizontalAlignment("center");
                    break;
                case 2:
                    selectRange.setHorizontalAlignment("normal");
                    break;
            }
        }
            
        if (style.verticalAlign !== null){
            switch (style.verticalAlign)
            {
                case 0:
                    selectRange.setVerticalAlignment("top");
                    break;
                case 1:
                    selectRange.setVerticalAlignment("middle");
                    break;
                case 2:
                    selectRange.setVerticalAlignment("bottom");
                    break;
            }
        }

        if (style.numberFormat !== null)
            selectRange.setNumberFormat(style.numberFormat);

        if (style.textRotation !== null)
            selectRange.setTextRotation(style.textRotation);

        if (style.backgroundColor !== null)
            selectRange.setBackgroundColor(style.backgroundColor);

        if (style.isWrap !== null && style.isWrap) {
            selectRange.setWrap(style.isWrap);
            if (style.wrapStrategy !== null)
                selectRange.setWrapStrategy(style.wrapStrategy);
        }
    })
}

export function setRangeBorders(snapshot, borders, ranges){
    var instanceId = snapshot.instanceId
    var activeSheet = selectSheet(instanceId, snapshot)
    if (!activeSheet) return

    ranges.forEach((range) => {
        var selectRange = activeSheet.getRange(range)
        borders.forEach((border) => selectRange.setBorder(border.type, border.style, border.color))
    })
}

export function getAllMerges(snapshot){
    var instanceId = snapshot.instanceId
    var sheet = selectSheet(instanceId, snapshot)
    if (!sheet) return []

    var result = []
    sheet.getMergedRanges().forEach(range => {
        result.push(range.getRange())
    })
    return result;
}

export function insertHyperLink(snapshot, text, link){
    var instanceId = snapshot.instanceId
    const range = selectRange(instanceId, snapshot)
    if (!range) return

    const api = getApi(instanceId)
    if (!api) return

    const richText = api.newRichText()
        .insertLink(text, link);

    range.setRichTextValueForCell(richText);
}

export async function insertComment(snapshot, comment){
    var instanceId = snapshot.instanceId
    const range = selectRange(instanceId, snapshot)
    if (!range) return

    const api = getApi(instanceId)
    if (!api) return

    const _comment = api.newTheadComment()
                                    .setContent(api.newRichText().insertText(comment.text.dataStream))
                                    .setDateTime(new Date(comment.dT))
                                    .setId(comment.id)
                                    .setPersonId(comment.userId)
    await range.addCommentAsync(_comment)
}

export function getAllComments(snapshot){
    var instanceId = snapshot.instanceId
    const comments = selectSheet(instanceId, snapshot).getComments()
    if (!comments) return []

    var result = []
    comments.forEach((comment) => { result.push(comment.getCommentData()) });
    return result;
}

export function getImagesId(snapshot){
    var instanceId = snapshot.instanceId
    const images = selectSheet(instanceId, snapshot).getImages()
    if (!images) return []

    var result = []
    images.forEach((img) => { result.push(img.getId()) })
    return result
}

export async function getImageById(snapshot, id, withSource){
    var instanceId = snapshot.instanceId
    var image = null
    await selectSheet(instanceId, snapshot).getImageById(id).toBuilder().buildAsync().then(result => {
        if (!withSource){
            result.source = ""
        }
        image = result
    })
    return image
}

export function addConditionalFormat(snapshot, queue)
{
    var instanceId = snapshot.instanceId
    var rule = getAndExecuteMethod(instanceId, queue, true)
    selectSheet(instanceId, snapshot).addConditionalFormattingRule(rule.res)
}

export function initListenerObject(instanceId, dotNetLstnr){
    var session = getSession(instanceId)
    if (!session) return
    session.listenerNET = { net: dotNetLstnr, listeners: [] }
}

export function addListenerRange(instanceId, listener) {
    var session = getSession(instanceId)
    if (!session || !session.listenerNET) return
    session.listenerNET.listeners.push(listener)
}

export function removeListener(instanceId, listener) {
    var session = getSession(instanceId)
    if (!session || !session.listenerNET) return
    var index = session.listenerNET.listeners.indexOf(listener)
    if (index > -1){
        session.listenerNET.listeners.splice(index, 1)
    }
}

function getWorksheetPermission(instanceId, snapshot){
    return selectSheet(instanceId, snapshot).getWorksheetPermission()
}

function rangesEqual(first, second){
    return first.startRow === second.startRow
        && first.endRow === second.endRow
        && first.startColumn === second.startColumn
        && first.endColumn === second.endColumn
}

export async function protectRangesInSheet(snapshot, configs){
    var instanceId = snapshot.instanceId
    const sheet = selectSheet(instanceId, snapshot)
    if (!sheet) return

    const permission = getWorksheetPermission(instanceId, snapshot)

    const payload = (configs ?? []).map(cfg => ({
        ranges: (cfg.ranges ?? []).map(range => sheet.getRange(range)),
        options: cfg.options ?? undefined
    }))

    await permission.protectRanges(payload)
}

export async function getProtectedRangesInSheet(snapshot){
    var instanceId = snapshot.instanceId
    const rules = await getWorksheetPermission(instanceId, snapshot).listRangeProtectionRules()
    return rules.map(rule => ({
        ruleId: rule.id,
        ranges: rule.ranges.map(range => range.getRange()),
        options: rule.options
    }))
}

export async function unprotectRuleIdsInSheet(snapshot, ruleIds){
    var instanceId = snapshot.instanceId
    await getWorksheetPermission(instanceId, snapshot).unprotectRules(ruleIds ?? [])
}

export async function isActiveRangeLocked(snapshot){
    var instanceId = snapshot.instanceId
    const selected = selectRange(instanceId, snapshot).getRange()
    const rules = await getWorksheetPermission(instanceId, snapshot).listRangeProtectionRules()

    return rules.some(rule => rule.ranges.some(range => rangesEqual(range.getRange(), selected)))
}

export function batchSheetOperations(instanceId, sheetId, operations) {
    var api = getApi(instanceId);
    if (!api) return [];
    var sheet = api.getActiveWorkbook().getSheetBySheetId(sheetId);
    if (!sheet) return [];
    var results = [];
    for (var i = 0; i < operations.length; i++) {
        var op = operations[i];
        var target = op.range ? sheet.getRange(op.range) : sheet;
        var method = target[op.method];
        if (typeof method === 'function')
            results.push(method.apply(target, op.args || []));
    }
    return results;
}

export function removeAllEvents(instanceId) {
    var api = getApi(instanceId);
    if (!api) return;
    try { api.removeEvent(api.Event.SheetEditEnded); } catch (e) { }
}

export async function setActiveRangeLock(snapshot, isLocked, options){
    var instanceId = snapshot.instanceId
    const selected = selectRange(instanceId, snapshot).getRange()
    const sheet = selectSheet(instanceId, snapshot)
    if (!sheet) return

    const permission = getWorksheetPermission(instanceId, snapshot)

    if (isLocked){
        await permission.protectRanges([{
            ranges: [sheet.getRange(selected)],
            options: options ?? { allowEdit: false }
        }])
        return
    }

    const rules = await permission.listRangeProtectionRules()
    const toDelete = []
    rules.forEach(rule => {
        if (rule.ranges.some(range => rangesEqual(range.getRange(), selected)))
            toDelete.push(rule.id)
    })

    if (toDelete.length > 0)
        await permission.unprotectRules(toDelete)
}
