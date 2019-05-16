<template>
    <v-layout column class="overflow-auto">
        <v-toolbar color="transparent" flat dense>
            <v-tabs color="transparent"
                v-model="model"
                centered
                slider-color="yellow">
                <v-tab ripple>
                    Единицы работы
                </v-tab>
                <v-tab ripple>
                    Спринты
                </v-tab>
                <v-tab ripple>
                    Поиск
                </v-tab>
            </v-tabs>
            <v-spacer></v-spacer>
            
        </v-toolbar>
        <v-tabs-items v-model="model">
            <v-tab-item>
                <work-items-tree/>
            </v-tab-item>
            <v-tab-item>
                <sprints/>
            </v-tab-item>
            <v-tab-item>
                Search
            </v-tab-item>
        </v-tabs-items>
    </v-layout>
</template>

<script>
import workItemsTree from './work-items-tree'
import sprints from './sprints'
export default { 
    components:{
        'sprints':sprints,
        'work-items-tree':workItemsTree
    },
    data:()=>({
        model:null
    })
}
</script>

