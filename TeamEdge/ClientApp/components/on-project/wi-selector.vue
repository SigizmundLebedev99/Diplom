<template>
    <v-menu :close-on-content-click="false" v-model="model">
        <template v-slot:activator="{ on }">
            <v-btn icon v-on="on" @click="fetchItems" flat small>
                <v-icon small>{{icon?icon:'add'}}</v-icon>
            </v-btn>
        </template>
        <div class="constraint white">
            <div class="block">
                <v-text-field prepend-icon="search" class="mx-2 my-0 pt-1 pb-0" v-model="filterText"></v-text-field>
            </div>
            <span class="ml-3" v-if="message">{{message}}</span>
            <v-list dense class="pt-0">
                <v-list-tile v-for="(item, i) in filteredItems" :key="i" @click="select(item)">
                    <span>{{`${item.code}${item.number} - ${item.name}`}}</span>
                </v-list-tile>
            </v-list>
        </div>
    </v-menu>
</template>

<script>
export default {
    props:[
        'icon',
        'code',
        'hasNoParent',
        'filter',
        'forSprint'
    ],
    data:()=>({
        items:[],
        filterText:'',
        model:false,
        message:null
    }),
    methods:{
        fetchItems(){
            this.message = null;
            var string = `/api/workitems/project/${this.$route.params.projId}/items?`
            if(this.code)
                string = string + `code=${this.code}`
            if(this.hasNoParent != null)
                string = string + `hasNoParent=${this.hasNoParent}`
            if(this.forSprint){
                string = `/api/workitems/project/${this.$route.params.projId}/for-sprint`
            }
            this.$http.get(string).then(
                r=>{
                    this.items = r.data;
                    if(!this.items.length)
                        this.message = "Нет подходящих единиц"
                },
                r=>{console.log(r.response)}
            );
        },
        select(item){
            this.$emit('selected', item);
            this.model = false;
        }
    },
    computed:{
        filteredItems(){
            var items = this.items;
            if(this.filter)
                items = items.filter(this.filter);
            if(this.filterText)
                items = items.filter(i=>i.name.startsWith(this.filterText))
            return items;
        }
    }
}
</script>

<style scoped>
    .block{
        max-width: 200px;
    }
    .constraint{
        max-height: 300px;
    }
</style>

