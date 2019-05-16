<template>
    <v-container class="mx-0 pt-0 scroll-y">
        <v-btn small class="text-none" @click="openSprintForm()">
            <v-icon>add</v-icon>
            Создать спринт
            </v-btn>
        <create-sprint @done="fetchItems()"/>
        <v-layout justify-center class="mt-3" v-if="!loading && !sprints.length">
            <span class="title">Пока в проекте не создано ни одного спринта</span>
        </v-layout>
        <v-treeview :items="sprints" item-key="descriptionId" :open="ids">
            <template v-slot:label="{ item }">
                <v-layout align-center v-if="!item.code">
                    <span>Спринт {{item.number}}</span>
                    <v-btn icon small class="ml-5">
                        <v-icon small class="ml-2" @click="editScript(item)">edit</v-icon>
                    </v-btn>
                </v-layout>
                <v-layout align-center v-else>
                <item-chip :item="item"/>
                <v-spacer/>
                    <v-chip v-if="item.status == 3" label small class="green lighten-1 white--text">Готово</v-chip>
                    <v-chip v-if="item.status == 2" label small class="red lighten-1 white--text">Приостановлено</v-chip>
                </v-layout>
            </template>
        </v-treeview>
    </v-container>
</template>

<script>
import createSprint from '../create-sprint'
import itemChip from '../item-chip'
import {mapGetters, mapMutations, mapActions} from 'vuex'
export default {
    components:{
        'create-sprint':createSprint,
        'item-chip':itemChip
    },
    methods:{
        ...mapMutations({
            openSprintForm:'backlog/openSprintForm'
        }),
        editScript(sprint){
            this.openSprintForm(sprint);
        },
        ...mapActions({
            fetchItems:'backlog/fetchItems'
        })
    },
    computed:{
        ...mapGetters({
            items:'backlog/items',
            sprints:'backlog/sprints',
            loading:'backlog/sprintsLoading'
        }),
        workItems(){
            return workItems;
        },
        ids(){
            return this.sprints.map(e=>e.descriptionId);
        }
    }
}
</script>


