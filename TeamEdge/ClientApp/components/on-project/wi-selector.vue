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
            <v-list dense class="pt-0">
                <v-list-tile v-for="(item, i) in items" :key="i" @click="select(item)">
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
        'hasNoParent'
    ],
    data:()=>({
        items:[],
        filterText:'',
        model:false
    }),
    methods:{
        fetchItems(){
            var string = `/api/workitems/project/${this.$route.params.projId}/items?`
            if(this.code)
                string = string + `code=${this.code}`
            if(this.hasNoParent != null)
                string = string + `hasNoParent=${this.hasNoParent}`
            this.$http.get(string).then(
                r=>{this.items = r.data},
                r=>{console.log(r.response)}
            );
        },
        select(item){
            this.$emit('selected', item);
            this.model = false;
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

